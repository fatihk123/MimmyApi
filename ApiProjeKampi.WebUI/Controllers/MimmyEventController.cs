using ApiProjeKampi.WebUI.Dtos.MimmyEvetDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ApiProjeKampi.WebUI.Controllers
{
    public class MimmyEventController : ApiProjeKampi.WebUI.Infrastructure.ApiUiController
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public MimmyEventController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<IActionResult> MimmyEventList()
        {
            var client = _httpClientFactory.CreateClient("Api");
            var responseMessage = await client.GetAsync("api/MimmyEvents");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultMimmyEventDto>>(jsonData);
                return View(values);
            }


            return View(new List<ResultMimmyEventDto>());
        }
        [HttpGet]
        public IActionResult CreateMimmyEvent()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> CreateMimmyEvent(CreateMimmyEventDtos createMimmyEventDtos)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var jsonData = JsonConvert.SerializeObject(createMimmyEventDtos);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("api/MimmyEvents", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("MimmyEventList");
            }
            await AddApiErrorsAsync(responseMessage);
            return View(createMimmyEventDtos);

        }

        [HttpPost]
        public async Task<IActionResult> DeleteMimmyEvent(int id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var responseMessage = await client.DeleteAsync("api/MimmyEvents?id=" + id);
            if (!responseMessage.IsSuccessStatusCode) return StatusCode((int)responseMessage.StatusCode);
            return RedirectToAction("MimmyEventList");


        }

        [HttpGet]
        public async Task<IActionResult> UpdateMimmyEvent(int id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var responseMessage = await client.GetAsync("api/MimmyEvents/GetMimmyEvent?id=" + id);
            if (!responseMessage.IsSuccessStatusCode) return StatusCode((int)responseMessage.StatusCode);

            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<UpdateMimmyEventDtos>(jsonData);
            return View(value);


        }
        [HttpPost]
        public async Task<IActionResult> UpdateMimmyEvent(UpdateMimmyEventDtos updateMimmyEventDtos)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var jsonData = JsonConvert.SerializeObject(updateMimmyEventDtos);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("api/MimmyEvents", stringContent);
            if (!responseMessage.IsSuccessStatusCode) { await AddApiErrorsAsync(responseMessage); return View(updateMimmyEventDtos); }

            return RedirectToAction("MimmyEventList");

        }
    }
}
