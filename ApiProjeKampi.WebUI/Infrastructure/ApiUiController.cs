using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjeKampi.WebUI.Infrastructure;

public abstract class ApiUiController : Controller
{
    protected async Task AddApiErrorsAsync(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        try
        {
            using var document = JsonDocument.Parse(content);
            var root = document.RootElement;
            if (root.ValueKind == JsonValueKind.Array)
                foreach (var error in root.EnumerateArray()) ModelState.AddModelError("", error.GetString() ?? "İşlem başarısız.");
            else if (root.ValueKind == JsonValueKind.Object && root.TryGetProperty("errors", out var errors))
                foreach (var field in errors.EnumerateObject())
                    foreach (var error in field.Value.EnumerateArray()) ModelState.AddModelError(field.Name, error.GetString() ?? "Geçersiz değer.");
            else ModelState.AddModelError("", "İşlem tamamlanamadı. Bilgileri kontrol edin.");
        }
        catch (JsonException)
        {
            ModelState.AddModelError("", content.Length is > 0 and < 400 ? content : "İşlem tamamlanamadı. Bilgileri kontrol edin.");
        }
    }
}
