namespace ApiProjeKampi.WebUI.Services;

public sealed class AiTextService(OpenAiService openAi, GeminiService gemini, IConfiguration configuration)
{
    private string Provider => (configuration["AI:Provider"] ?? "Gemini").Trim();
    public string ProviderName => Provider.Equals("OpenAI", StringComparison.OrdinalIgnoreCase) ? "OpenAI" : Provider.Equals("Gemini", StringComparison.OrdinalIgnoreCase) ? "Gemini" : "Bilinmeyen sağlayıcı";
    public bool IsConfigured => ProviderName switch { "OpenAI" => openAi.IsConfigured, "Gemini" => gemini.IsConfigured, _ => false };

    public Task<string> GenerateAsync(string instructions, string prompt, CancellationToken cancellationToken) => ProviderName switch
    {
        "OpenAI" => openAi.GenerateAsync(instructions, prompt, cancellationToken),
        "Gemini" => gemini.GenerateAsync(instructions, prompt, cancellationToken),
        _ => throw new AiServiceException("AI:Provider ayarı Gemini veya OpenAI olmalıdır.")
    };
}
