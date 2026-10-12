using ApiProjeKampi.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
namespace ApiProjeKampi.WebUI.ViewComponents;

public class _HomePageStatisticsComponentPartial(IHttpClientFactory httpClientFactory) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var model = await httpClientFactory.CreateClient("Api").GetFromJsonAsync<DashboardViewModel>("api/Statistics/Summary");
        return View(model ?? new DashboardViewModel());
    }
}
