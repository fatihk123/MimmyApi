using ApiProjeKampi.WebUI.Dtos.ServiceDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ApiProjeKampi.WebUI.Controllers
{
    public class WhyChooseMimmyController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public WhyChooseMimmyController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<IActionResult> WhyChooseMimmyList()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7192/api/Services");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultWhyChooseMimmyDto>>(jsonData);
                return View(values);
            }


            return View();
        }
        [HttpGet]
        public IActionResult CreateWhyChooseMimmy()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> CreateWhyChooseMimmy(CreateWhyChooseMimmyDto createWhyChooseMimmyDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(createWhyChooseMimmyDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("https://localhost:7192/api/Services", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("WhyChooseMimmyList");
            }
            return View();

        }

        public async Task<IActionResult> DeleteWhyChooseMimmy(int id)
        {
            var client = _httpClientFactory.CreateClient();
            await client.DeleteAsync("https://localhost:7192/api/Services?id=" + id);
            return RedirectToAction("WhyChooseMimmyList");


        }

        [HttpGet]
        public async Task<IActionResult> UpdateWhyChooseMimmy(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7192/api/Services/GetService?id=" + id);

            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<GetWhyChooseMimmyByIdDto>(jsonData);
            return View(value);


        }
        [HttpPost]
        public async Task<IActionResult> UpdateWhyChooseMimmy(UpdateWhyChooseMimmyDto updateWhyChooseMimmyDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(updateWhyChooseMimmyDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            await client.PutAsync("https://localhost:7192/api/Services", stringContent);

            return RedirectToAction("WhyChooseMimmyList");

        }
    }
}
