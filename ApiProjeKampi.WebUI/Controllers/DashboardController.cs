using ApiProjeKampi.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
namespace ApiProjeKampi.WebUI.Controllers;

public class DashboardController(IHttpClientFactory httpClientFactory) : Controller
{
    public async Task<IActionResult> Index()
    {
        var model = await httpClientFactory.CreateClient("Api").GetFromJsonAsync<DashboardViewModel>("api/Statistics/Summary");
        return View(model ?? new DashboardViewModel());
    }
}
