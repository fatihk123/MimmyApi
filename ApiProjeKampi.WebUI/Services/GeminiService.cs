using System.Net;
using System.Text.Json;

namespace ApiProjeKampi.WebUI.Services;

public sealed class GeminiService(HttpClient client, IConfiguration configuration)
{
    public bool IsConfigured => !string.IsNullOrWhiteSpace(ApiKey);
    private string? ApiKey => configuration["Gemini:ApiKey"] ?? configuration["GEMINI_API_KEY"];

    public async Task<string> GenerateAsync(string instructions, string prompt, CancellationToken cancellationToken)
    {
        if (!IsConfigured) throw new AiServiceException("Gemini API anahtarı henüz yapılandırılmadı. Google AI Studio ile bir anahtar oluşturup README adımlarıyla ekleyin.");
        var model = configuration["Gemini:Model"] ?? "gemini-3.5-flash-lite";
        using var request = new HttpRequestMessage(HttpMethod.Post, $"v1beta/models/{Uri.EscapeDataString(model)}:generateContent");
        request.Headers.Add("x-goog-api-key", ApiKey);
        request.Content = JsonContent.Create(new
        {
            systemInstruction = new { parts = new[] { new { text = instructions } } },
            contents = new[] { new { role = "user", parts = new[] { new { text = prompt } } } },
            generationConfig = new { maxOutputTokens = 1600 }
        });
        try
        {
            using var response = await client.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var message = response.StatusCode switch
                {
                    HttpStatusCode.BadRequest or HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden => "Gemini anahtarı veya model erişimi doğrulanamadı. Google AI Studio ayarlarını kontrol edin.",
                    HttpStatusCode.NotFound => "Seçili Gemini modeli bulunamadı. Gemini:Model ayarını kontrol edin.",
                    HttpStatusCode.TooManyRequests => "Gemini ücretsiz kullanım kotası veya istek limiti doldu. Daha sonra tekrar deneyin.",
                    _ => "Gemini şu anda yanıt veremiyor. Daha sonra tekrar deneyin."
                };
                throw new AiServiceException(message);
            }
            using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync(cancellationToken));
            var root = document.RootElement;
            if (root.ValueKind != JsonValueKind.Object) throw new AiServiceException("Gemini yanıtı okunamadı. Tekrar deneyin.");
            if (root.TryGetProperty("promptFeedback", out var feedback) && feedback.ValueKind == JsonValueKind.Object && feedback.TryGetProperty("blockReason", out _))
                throw new AiServiceException("Gemini bu isteğe yanıt veremedi. İsteğinizi yeniden ifade edin.");
            var parts = new List<string>();
            if (root.TryGetProperty("candidates", out var candidates) && candidates.ValueKind == JsonValueKind.Array && candidates.GetArrayLength() > 0)
            {
                var candidate = candidates[0];
                if (candidate.ValueKind == JsonValueKind.Object && candidate.TryGetProperty("finishReason", out var reason) && reason.ValueKind == JsonValueKind.String && reason.GetString() == "SAFETY")
                    throw new AiServiceException("Gemini bu isteğe yanıt veremedi. İsteğinizi yeniden ifade edin.");
                if (candidate.ValueKind == JsonValueKind.Object && candidate.TryGetProperty("content", out var content) && content.ValueKind == JsonValueKind.Object && content.TryGetProperty("parts", out var contentParts) && contentParts.ValueKind == JsonValueKind.Array)
                    foreach (var part in contentParts.EnumerateArray())
                        if (part.ValueKind == JsonValueKind.Object && (!part.TryGetProperty("thought", out var thought) || thought.ValueKind != JsonValueKind.True) && part.TryGetProperty("text", out var text) && text.ValueKind == JsonValueKind.String)
                            parts.Add(text.GetString() ?? "");
            }
            var result = string.Join("\n", parts).Trim();
            if (result.Length == 0) throw new AiServiceException("Gemini boş yanıt döndürdü. Tekrar deneyin.");
            return result;
        }
        catch (HttpRequestException) { throw new AiServiceException("Gemini servisine bağlanılamadı. İnternet bağlantısını kontrol edin."); }
        catch (JsonException) { throw new AiServiceException("Gemini yanıtı okunamadı. Tekrar deneyin."); }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested) { throw new AiServiceException("Gemini yanıtı zaman aşımına uğradı. Tekrar deneyin."); }
    }
}
