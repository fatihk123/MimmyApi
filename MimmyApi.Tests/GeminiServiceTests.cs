using System.Net;
using System.Text;
using System.Text.Json;
using ApiProjeKampi.WebUI.Services;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace MimmyApi.Tests;

public class GeminiServiceTests
{
    private const string ValidResponse = """
    {"candidates":[{"content":{"parts":[{"thought":true,"text":"internal notes"},{"text":"Domatesli makarna"},{"text":"Malzemeleri hazırlayın."}]},"finishReason":"STOP"}]}
    """;
    private static IConfiguration Config(bool configured = true, string? provider = null) => new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
    {
        ["Gemini:ApiKey"] = configured ? "test-only-key" : null,
        ["Gemini:Model"] = "test-model",
        ["AI:Provider"] = provider
    }).Build();
    private static GeminiService Create(StubHandler handler, bool configured = true) => new(new HttpClient(handler) { BaseAddress = new Uri("https://generativelanguage.googleapis.com/") }, Config(configured));

    [Fact]
    public async Task MissingKeyDoesNotSendRequest()
    {
        var handler = new StubHandler(_ => Task.FromResult(Response(ValidResponse)));
        await Assert.ThrowsAsync<AiServiceException>(() => Create(handler, false).GenerateAsync("system", "prompt", default));
        Assert.Equal(0, handler.Calls);
    }

    [Fact]
    public async Task RequestKeepsKeyOutOfUrlAndParsesAnswerOnly()
    {
        var handler = new StubHandler(async request =>
        {
            Assert.Equal("https://generativelanguage.googleapis.com/v1beta/models/test-model:generateContent", request.RequestUri!.ToString());
            Assert.Equal("test-only-key", Assert.Single(request.Headers.GetValues("x-goog-api-key")));
            Assert.DoesNotContain("test-only-key", request.RequestUri.ToString());
            using var body = JsonDocument.Parse(await request.Content!.ReadAsStringAsync());
            Assert.Equal("system", body.RootElement.GetProperty("systemInstruction").GetProperty("parts")[0].GetProperty("text").GetString());
            Assert.Equal("ingredients", body.RootElement.GetProperty("contents")[0].GetProperty("parts")[0].GetProperty("text").GetString());
            return Response(ValidResponse);
        });
        Assert.Equal("Domatesli makarna\nMalzemeleri hazırlayın.", await Create(handler).GenerateAsync("system", "ingredients", default));
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest, "anahtarı")]
    [InlineData(HttpStatusCode.Unauthorized, "anahtarı")]
    [InlineData(HttpStatusCode.Forbidden, "anahtarı")]
    [InlineData(HttpStatusCode.NotFound, "modeli")]
    [InlineData(HttpStatusCode.TooManyRequests, "kota")]
    [InlineData(HttpStatusCode.InternalServerError, "yanıt veremiyor")]
    public async Task ProviderErrorsHaveSafeMessages(HttpStatusCode status, string expected)
    {
        var handler = new StubHandler(_ => Task.FromResult(new HttpResponseMessage(status) { Content = new StringContent("private diagnostic") }));
        var error = await Assert.ThrowsAsync<AiServiceException>(() => Create(handler).GenerateAsync("system", "prompt", default));
        Assert.Contains(expected, error.Message);
        Assert.DoesNotContain("private diagnostic", error.Message);
    }

    [Theory]
    [InlineData("{invalid", "okunamadı")]
    [InlineData("[]", "okunamadı")]
    [InlineData("{\"candidates\":[]}", "boş yanıt")]
    [InlineData("{\"promptFeedback\":{\"blockReason\":\"SAFETY\"}}", "yeniden ifade")]
    [InlineData("{\"candidates\":[{\"finishReason\":\"SAFETY\"}]}", "yeniden ifade")]
    public async Task InvalidEmptyOrBlockedResponsesAreReported(string json, string expected)
    {
        var handler = new StubHandler(_ => Task.FromResult(Response(json)));
        var error = await Assert.ThrowsAsync<AiServiceException>(() => Create(handler).GenerateAsync("system", "prompt", default));
        Assert.Contains(expected, error.Message);
    }

    [Fact]
    public async Task MissingGeminiKeyDoesNotFallBackToOpenAi()
    {
        var handler = new StubHandler(_ => throw new InvalidOperationException("No provider should be called"));
        var openAiConfig = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?> { ["OpenAI:ApiKey"] = "configured-openai-key" }).Build();
        var openAi = new OpenAiService(new HttpClient(handler) { BaseAddress = new Uri("https://api.openai.com/") }, openAiConfig);
        var selector = new AiTextService(openAi, Create(handler, false), Config(false));
        Assert.Equal("Gemini", selector.ProviderName);
        Assert.False(selector.IsConfigured);
        await Assert.ThrowsAsync<AiServiceException>(() => selector.GenerateAsync("system", "prompt", default));
        Assert.Equal(0, handler.Calls);
    }

    [Fact]
    public async Task UnknownProviderDoesNotMakeRequest()
    {
        var handler = new StubHandler(_ => throw new InvalidOperationException("No provider should be called"));
        var config = Config(true, "unknown");
        var selector = new AiTextService(new OpenAiService(new HttpClient(handler), config), Create(handler), config);
        Assert.False(selector.IsConfigured);
        await Assert.ThrowsAsync<AiServiceException>(() => selector.GenerateAsync("system", "prompt", default));
        Assert.Equal(0, handler.Calls);
    }

    private static HttpResponseMessage Response(string json) => new(HttpStatusCode.OK) { Content = new StringContent(json, Encoding.UTF8, "application/json") };
    private sealed class StubHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> send) : HttpMessageHandler
    {
        public int Calls { get; private set; }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) { Calls++; return send(request); }
    }
}
