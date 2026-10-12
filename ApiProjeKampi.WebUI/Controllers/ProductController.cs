using ApiProjeKampi.WebUI.Dtos.CategoryDtos;
using ApiProjeKampi.WebUI.Dtos.ProductDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ApiProjeKampi.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<IActionResult> ProductList()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7192/api/Products/GetProductsWithCategory");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultProductDto>>(jsonData);
                return View(values);
            }


            return View();
        }





        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7192/api/Categories");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(jsonData);
                List<SelectListItem> categoryValues = (from x in values
                                                       select new SelectListItem
                                                       {
                                                           Text = x.CategoryName,
                                                           Value = x.CategoryId.ToString()

                                                       }).ToList();
                ViewBag.Categories = categoryValues;

            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto createProductDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(createProductDto);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            await client.PostAsync("https://localhost:7192/api/Products", stringContent);


            return RedirectToAction("ProductList");

        }
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var client = _httpClientFactory.CreateClient();
            await client.DeleteAsync("https://localhost:7192/api/Products?id=" + id);
            return RedirectToAction("ProductList");


        }

        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7192/api/Products/GetById?id=" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<GetProductByIdDto>(jsonData);
                var categoryResponse = await client.GetAsync("https://localhost:7192/api/Categories");
                if (categoryResponse.IsSuccessStatusCode)
                {
                    var categoryJsonData = await categoryResponse.Content.ReadAsStringAsync();
                    var categories = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(categoryJsonData);
                    List<SelectListItem> categoryValues = (from x in categories
                                                           select new SelectListItem
                                                           {
                                                               Text = x.CategoryName,
                                                               Value = x.CategoryId.ToString()
                                                           }).ToList();
                    ViewBag.Categories = categoryValues;
                }
                return View(value);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProductDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(updateProductDto);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("https://localhost:7192/api/Products", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("ProductList");
            }
            return View();


        }
    }
}