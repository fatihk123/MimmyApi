using ApiProjeKampi.WebUI.Dtos.AboutDtos;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjeKampi.WebUI.ViewComponents;

public class _AboutDefaultComponentPartial(IHttpClientFactory httpClientFactory) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var values = await httpClientFactory.CreateClient("Api").GetFromJsonAsync<List<ResultAboutDto>>("api/Abouts");
        return View(values ?? new List<ResultAboutDto>());
    }
}
