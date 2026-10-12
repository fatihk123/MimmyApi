using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ApiProjeKampi.WebUI.Services;

public sealed class OpenAiService(HttpClient client, IConfiguration configuration)
{
    public bool IsConfigured => !string.IsNullOrWhiteSpace(ApiKey);
    private string? ApiKey => configuration["OpenAI:ApiKey"] ?? configuration["OPENAI_API_KEY"];

    public async Task<string> GenerateAsync(string instructions, string prompt, CancellationToken cancellationToken)
    {
        if (!IsConfigured) throw new AiServiceException("Yapay zekâ için OpenAI API anahtarı henüz yapılandırılmadı. README içindeki kurulum adımlarını izleyin.");
        using var request = new HttpRequestMessage(HttpMethod.Post, "v1/responses");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
        request.Content = JsonContent.Create(new
        {
            model = configuration["OpenAI:Model"] ?? "gpt-4.1-mini",
            instructions,
            input = prompt,
            max_output_tokens = 1200,
            store = false
        });
        try
        {
            using var response = await client.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var message = response.StatusCode switch
                {
                    HttpStatusCode.Unauthorized => "OpenAI API anahtarı geçersiz. Yapılandırmayı kontrol edin.",
                    HttpStatusCode.TooManyRequests => "OpenAI kullanım limiti veya kota sınırı aşıldı. Daha sonra tekrar deneyin.",
                    _ => "Yapay zekâ servisi şu anda yanıt veremiyor. Daha sonra tekrar deneyin."
                };
                throw new AiServiceException(message);
            }
            using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync(cancellationToken));
            var parts = new List<string>();
            if (document.RootElement.ValueKind == JsonValueKind.Object && document.RootElement.TryGetProperty("output", out var output) && output.ValueKind == JsonValueKind.Array)
                foreach (var item in output.EnumerateArray())
                    if (item.ValueKind == JsonValueKind.Object && item.TryGetProperty("content", out var content) && content.ValueKind == JsonValueKind.Array)
                        foreach (var part in content.EnumerateArray())
                            if (part.ValueKind == JsonValueKind.Object && part.TryGetProperty("type", out var type) && type.ValueKind == JsonValueKind.String && type.GetString() == "output_text" && part.TryGetProperty("text", out var text) && text.ValueKind == JsonValueKind.String)
                                parts.Add(text.GetString() ?? "");
            var result = string.Join("\n", parts).Trim();
            if (result.Length == 0) throw new AiServiceException("Yapay zekâ servisi boş yanıt döndürdü. Tekrar deneyin.");
            return result;
        }
        catch (HttpRequestException) { throw new AiServiceException("Yapay zekâ servisine bağlanılamadı. İnternet bağlantısını kontrol edin."); }
        catch (JsonException) { throw new AiServiceException("Yapay zekâ yanıtı okunamadı. Tekrar deneyin."); }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested) { throw new AiServiceException("Yapay zekâ yanıtı zaman aşımına uğradı. Tekrar deneyin."); }
    }
}

public sealed class AiServiceException(string message) : Exception(message);
