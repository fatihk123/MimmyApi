using ApiProjeKampi.WebUI.Dtos.CategoryDtos;
using ApiProjeKampi.WebUI.Dtos.ProductDtos;
using ApiProjeKampi.WebUI.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ApiProjeKampi.WebUI.Controllers;

public class ProductController(IHttpClientFactory httpClientFactory) : ApiUiController
{
    private HttpClient Api => httpClientFactory.CreateClient("Api");

    public async Task<IActionResult> ProductList() => View(await Api.GetFromJsonAsync<List<ResultProductDto>>("api/Products/GetProductsWithCategory") ?? new());

    private async Task LoadCategoriesAsync()
    {
        var categories = await Api.GetFromJsonAsync<List<ResultCategoryDto>>("api/Categories") ?? new();
        ViewBag.Categories = categories.Select(x => new SelectListItem(x.CategoryName, x.CategoryId.ToString())).ToList();
    }

    [HttpGet]
    public async Task<IActionResult> CreateProduct()
    {
        await LoadCategoriesAsync();
        return View(new CreateProductDto());
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductDto model)
    {
        if (ModelState.IsValid)
        {
            var response = await Api.PostAsJsonAsync("api/Products", model);
            if (response.IsSuccessStatusCode) return RedirectToAction(nameof(ProductList));
            await AddApiErrorsAsync(response);
        }
        await LoadCategoriesAsync();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var response = await Api.DeleteAsync("api/Products?id=" + id);
        if (!response.IsSuccessStatusCode) return StatusCode((int)response.StatusCode);
        return RedirectToAction(nameof(ProductList));
    }

    [HttpGet]
    public async Task<IActionResult> UpdateProduct(int id)
    {
        var response = await Api.GetAsync("api/Products/GetById?id=" + id);
        if (!response.IsSuccessStatusCode) return StatusCode((int)response.StatusCode);
        var model = await response.Content.ReadFromJsonAsync<UpdateProductDto>();
        await LoadCategoriesAsync();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProduct(UpdateProductDto model)
    {
        if (ModelState.IsValid)
        {
            var response = await Api.PutAsJsonAsync("api/Products", model);
            if (response.IsSuccessStatusCode) return RedirectToAction(nameof(ProductList));
            await AddApiErrorsAsync(response);
        }
        await LoadCategoriesAsync();
        return View(model);
    }
}
