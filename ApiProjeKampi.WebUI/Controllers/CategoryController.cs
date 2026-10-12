using ApiProjeKampi.WebUI.Dtos.CategoryDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ApiProjeKampi.WebUI.Controllers
{
    public class CategoryController : ApiProjeKampi.WebUI.Infrastructure.ApiUiController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CategoryController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<IActionResult> CategoryList()
        {
            var client = _httpClientFactory.CreateClient("Api");
            var responseMessage = await client.GetAsync("api/Categories");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(jsonData);
                return View(values);
            }


            return View(new List<ResultCategoryDto>());
        }
        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var jsonData = JsonConvert.SerializeObject(createCategoryDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("api/Categories", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("CategoryList");
            }
            await AddApiErrorsAsync(responseMessage);
            return View(createCategoryDto);

        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var responseMessage = await client.DeleteAsync("api/Categories?id=" + id);
            if (!responseMessage.IsSuccessStatusCode) return StatusCode((int)responseMessage.StatusCode);
            return RedirectToAction("CategoryList");


        }

        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var responseMessage = await client.GetAsync("api/Categories/GetCategory?id=" + id);
            if (!responseMessage.IsSuccessStatusCode) return StatusCode((int)responseMessage.StatusCode);

            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<UpdateCategoryDto>(jsonData);
            return View(value);


        }
        [HttpPost]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var jsonData = JsonConvert.SerializeObject(updateCategoryDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("api/Categories", stringContent);
            if (!responseMessage.IsSuccessStatusCode) { await AddApiErrorsAsync(responseMessage); return View(updateCategoryDto); }

            return RedirectToAction("CategoryList");

        }

    }
}
