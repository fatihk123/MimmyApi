using ApiProjeKampi.WebUI.Dtos.MimmyEvetDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ApiProjeKampi.WebUI.Controllers
{
    public class MimmyEventController : Controller
    {
       
            private readonly IHttpClientFactory _httpClientFactory;

            public MimmyEventController(IHttpClientFactory httpClientFactory)
            {
                _httpClientFactory = httpClientFactory;
            }


            public async Task<IActionResult> MimmyEventList()
            {
                var client = _httpClientFactory.CreateClient();
                var responseMessage = await client.GetAsync("https://localhost:7192/api/MimmyEvents");
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonData = await responseMessage.Content.ReadAsStringAsync();
                    var values = JsonConvert.DeserializeObject<List<ResultMimmyEventDto>>(jsonData);
                    return View(values);
                }


                return View();
            }
            [HttpGet]
            public IActionResult CreateMimmyEvent()
            {
                return View();

            }
            [HttpPost]
            public async Task<IActionResult> CreateMimmyEvent(CreateMimmyEventDtos createMimmyEventDtos)
            {
                var client = _httpClientFactory.CreateClient();
                var jsonData = JsonConvert.SerializeObject(createMimmyEventDtos);
                StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var responseMessage = await client.PostAsync("https://localhost:7192/api/MimmyEvents", stringContent);
                if (responseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("MimmyEventList");
                }
                return View();

            }

            public async Task<IActionResult> DeleteMimmyEvent(int id)
            {
                var client = _httpClientFactory.CreateClient();
                await client.DeleteAsync("https://localhost:7192/api/MimmyEvents?id=" + id);
                return RedirectToAction("MimmyEventList");


            }

            [HttpGet]
            public async Task<IActionResult> UpdateMimmyEvent(int id)
            {
                var client = _httpClientFactory.CreateClient();
                var responseMessage = await client.GetAsync("https://localhost:7192/api/MimmyEvents/GetMimmyEvent?id=" + id);

                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<GetMimmyEventByIdDtos>(jsonData);
                return View(value);


            }
            [HttpPost]
            public async Task<IActionResult> UpdateMimmyEvent(UpdateMimmyEventDtos updateMimmyEventDtos)
            {
                var client = _httpClientFactory.CreateClient();
                var jsonData = JsonConvert.SerializeObject(updateMimmyEventDtos);
                StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
                await client.PutAsync("https://localhost:7192/api/MimmyEvents", stringContent);

                return RedirectToAction("MimmyEventList");

            }
        }
}
