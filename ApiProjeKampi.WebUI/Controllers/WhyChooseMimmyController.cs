using ApiProjeKampi.WebUI.Dtos.ServiceDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ApiProjeKampi.WebUI.Controllers
{
    public class WhyChooseMimmyController : ApiProjeKampi.WebUI.Infrastructure.ApiUiController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public WhyChooseMimmyController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<IActionResult> WhyChooseMimmyList()
        {
            var client = _httpClientFactory.CreateClient("Api");
            var responseMessage = await client.GetAsync("api/Services");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultWhyChooseMimmyDto>>(jsonData);
                return View(values);
            }


            return View(new List<ResultWhyChooseMimmyDto>());
        }
        [HttpGet]
        public IActionResult CreateWhyChooseMimmy()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> CreateWhyChooseMimmy(CreateWhyChooseMimmyDto createWhyChooseMimmyDto)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var jsonData = JsonConvert.SerializeObject(createWhyChooseMimmyDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("api/Services", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("WhyChooseMimmyList");
            }
            await AddApiErrorsAsync(responseMessage);
            return View(createWhyChooseMimmyDto);

        }

        [HttpPost]
        public async Task<IActionResult> DeleteWhyChooseMimmy(int id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var responseMessage = await client.DeleteAsync("api/Services?id=" + id);
            if (!responseMessage.IsSuccessStatusCode) return StatusCode((int)responseMessage.StatusCode);
            return RedirectToAction("WhyChooseMimmyList");


        }

        [HttpGet]
        public async Task<IActionResult> UpdateWhyChooseMimmy(int id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var responseMessage = await client.GetAsync("api/Services/GetService?id=" + id);
            if (!responseMessage.IsSuccessStatusCode) return StatusCode((int)responseMessage.StatusCode);

            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<UpdateWhyChooseMimmyDto>(jsonData);
            return View(value);


        }
        [HttpPost]
        public async Task<IActionResult> UpdateWhyChooseMimmy(UpdateWhyChooseMimmyDto updateWhyChooseMimmyDto)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var jsonData = JsonConvert.SerializeObject(updateWhyChooseMimmyDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("api/Services", stringContent);
            if (!responseMessage.IsSuccessStatusCode) { await AddApiErrorsAsync(responseMessage); return View(updateWhyChooseMimmyDto); }

            return RedirectToAction("WhyChooseMimmyList");

        }
    }
}
