using ApiProjeKampi.WebUI.Dtos.CategoryDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiProjeKampi.WebUI.ViewComponents.DefaultMenuViewComponentsPartial
{
    public class _DefaultMenuCategoryComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _DefaultMenuCategoryComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7192/api/Categories");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var value=JsonConvert.DeserializeObject<List<ResultCategoryDto>>(jsonData);
                return View(value);

            }


                return View();
        }

    }
}
