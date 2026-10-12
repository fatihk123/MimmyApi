using ApiProjeKampi.WebUI.Dtos.FeatureDtos;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjeKampi.WebUI.ViewComponents;

public class _FeatureDefaultComponentPartial(IHttpClientFactory httpClientFactory) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var values = await httpClientFactory.CreateClient("Api").GetFromJsonAsync<List<ResultFeatureDto>>("api/Features");
        return View(values ?? new List<ResultFeatureDto>());
    }
}
