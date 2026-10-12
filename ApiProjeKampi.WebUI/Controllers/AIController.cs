using ApiProjeKampi.WebUI.Models;
using ApiProjeKampi.WebUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjeKampi.WebUI.Controllers;

public class AIController(OpenAiService ai) : Controller
{
    [HttpGet]
    public IActionResult CreateRecipeWithOpenAI() => View(new AiRequestViewModel { IsConfigured = ai.IsConfigured });

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateRecipeWithOpenAI(AiRequestViewModel model, CancellationToken cancellationToken)
    {
        model.IsConfigured = ai.IsConfigured;
        if (!ModelState.IsValid) return View(model);
        try
        {
            model.Result = await ai.GenerateAsync("Mimmy restoranının Türkçe tarif asistanısın. Kullanıcının malzemelerine göre bir yemek öner. Başlık, porsiyon, hazırlık süresi, malzemeler ve numaralı hazırlık adımları ver. Alerjenleri belirt. Ek malzeme gerekiyorsa açıkça belirt. HTML üretme.", model.Prompt, cancellationToken);
        }
        catch (AiServiceException error) { ModelState.AddModelError("", error.Message); }
        return View(model);
    }

    [HttpGet]
    public IActionResult DailyMenu() => View("CreateRecipeWithOpenAI", new AiRequestViewModel { Prompt = "Türkiye, İtalya, Fransa ve Çin mutfaklarından dört farklı günlük menü öner.", IsConfigured = ai.IsConfigured });
}
