using System.Net;
using System.Text;
using System.Text.Json;
using ApiProjeKampi.WebUI.Services;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace MimmyApi.Tests;

public class OpenAiServiceTests
{
    private const string ValidResponse = """
    {"output":[{"type":"reasoning","summary":[]},{"type":"message","content":[{"type":"output_text","text":"Domatesli makarna"},{"type":"output_text","text":"1. Malzemeleri hazırlayın."}]}]}
    """;

    private static OpenAiService Create(StubHandler handler, bool configured = true)
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["OpenAI:ApiKey"] = configured ? "test-only-key" : null,
            ["OpenAI:Model"] = "test-model"
        }).Build();
        return new OpenAiService(new HttpClient(handler) { BaseAddress = new Uri("https://api.openai.com/") }, configuration);
    }

    [Fact]
    public async Task UnconfiguredServiceDoesNotSendRequest()
    {
        var handler = new StubHandler(_ => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));
        var error = await Assert.ThrowsAsync<AiServiceException>(() => Create(handler, false).GenerateAsync("instructions", "prompt", default));
        Assert.Contains("anahtarı", error.Message);
        Assert.Equal(0, handler.Calls);
    }

    [Fact]
    public async Task RequestUsesConfiguredModelAndDoesNotStoreContent()
    {
        var handler = new StubHandler(async request =>
        {
            Assert.Equal("https://api.openai.com/v1/responses", request.RequestUri!.ToString());
            Assert.Equal("Bearer", request.Headers.Authorization!.Scheme);
            using var body = JsonDocument.Parse(await request.Content!.ReadAsStringAsync());
            Assert.Equal("test-model", body.RootElement.GetProperty("model").GetString());
            Assert.Equal("malzemeler", body.RootElement.GetProperty("input").GetString());
            Assert.False(body.RootElement.GetProperty("store").GetBoolean());
            return JsonResponse(ValidResponse);
        });
        var result = await Create(handler).GenerateAsync("instructions", "malzemeler", default);
        Assert.Equal("Domatesli makarna\n1. Malzemeleri hazırlayın.", result);
    }

    [Theory]
    [InlineData(HttpStatusCode.Unauthorized, "geçersiz")]
    [InlineData(HttpStatusCode.TooManyRequests, "kota")]
    [InlineData(HttpStatusCode.InternalServerError, "yanıt veremiyor")]
    public async Task ProviderErrorsHaveSafeMessages(HttpStatusCode status, string expected)
    {
        var handler = new StubHandler(_ => Task.FromResult(new HttpResponseMessage(status) { Content = new StringContent("private provider diagnostic") }));
        var error = await Assert.ThrowsAsync<AiServiceException>(() => Create(handler).GenerateAsync("instructions", "prompt", default));
        Assert.Contains(expected, error.Message);
        Assert.DoesNotContain("private", error.Message);
    }

    [Theory]
    [InlineData("{invalid", "okunamadı")]
    [InlineData("[]", "boş yanıt")]
    [InlineData("{\"output\":\"unexpected\"}", "boş yanıt")]
    [InlineData("{\"output\":[]}", "boş yanıt")]
    public async Task InvalidOrEmptyResponsesAreReported(string json, string expected)
    {
        var handler = new StubHandler(_ => Task.FromResult(JsonResponse(json)));
        var error = await Assert.ThrowsAsync<AiServiceException>(() => Create(handler).GenerateAsync("instructions", "prompt", default));
        Assert.Contains(expected, error.Message);
    }

    [Fact]
    public async Task NetworkFailureHasFriendlyMessage()
    {
        var handler = new StubHandler(_ => throw new HttpRequestException("private network details"));
        var error = await Assert.ThrowsAsync<AiServiceException>(() => Create(handler).GenerateAsync("instructions", "prompt", default));
        Assert.Contains("bağlanılamadı", error.Message);
    }

    [Fact]
    public async Task CallerCancellationIsPreserved()
    {
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        var handler = new StubHandler(_ => throw new OperationCanceledException(cancellation.Token));
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => Create(handler).GenerateAsync("instructions", "prompt", cancellation.Token));
    }

    [Fact]
    public async Task TimeoutHasFriendlyMessage()
    {
        var handler = new StubHandler(_ => throw new TaskCanceledException());
        var error = await Assert.ThrowsAsync<AiServiceException>(() => Create(handler).GenerateAsync("instructions", "prompt", default));
        Assert.Contains("zaman aşımına", error.Message);
    }

    private static HttpResponseMessage JsonResponse(string json) => new(HttpStatusCode.OK) { Content = new StringContent(json, Encoding.UTF8, "application/json") };
    private sealed class StubHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> send) : HttpMessageHandler
    {
        public int Calls { get; private set; }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Calls++;
            return send(request);
        }
    }
}
