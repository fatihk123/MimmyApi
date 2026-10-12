using ApiProjeKampi.WebUI.Dtos.CategoryDtos;
using ApiProjeKampi.WebUI.Dtos.ProductDtos;
using ApiProjeKampi.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
namespace ApiProjeKampi.WebUI.ViewComponents.DefaultMenuViewComponents;

public class _DefaultMenuViewComponentPatrial(IHttpClientFactory httpClientFactory) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var client = httpClientFactory.CreateClient("Api");
        var categories = await client.GetFromJsonAsync<List<ResultCategoryDto>>("api/Categories");
        var products = await client.GetFromJsonAsync<List<ResultProductDto>>("api/Products");
        return View(new MenuViewModel { Categories = categories ?? new(), Products = products ?? new() });
    }
}
