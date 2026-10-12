using ApiProjeKampi.WebUI.Dtos.ContactDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ApiProjeKampi.WebUI.Controllers
{
    public class ContactController : ApiProjeKampi.WebUI.Infrastructure.ApiUiController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ContactController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<IActionResult> ContactList()
        {
            var client = _httpClientFactory.CreateClient("Api");
            var responseMessage = await client.GetAsync("api/Contacts");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultContactDto>>(jsonData);
                return View(values);
            }


            return View(new List<ResultContactDto>());
        }
        [HttpGet]
        public IActionResult CreateContact()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> CreateContact(CreateContactDto createContactDto)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var jsonData = JsonConvert.SerializeObject(createContactDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("api/Contacts", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("ContactList");
            }
            await AddApiErrorsAsync(responseMessage);
            return View(createContactDto);

        }

        [HttpPost]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var responseMessage = await client.DeleteAsync("api/Contacts?id=" + id);
            if (!responseMessage.IsSuccessStatusCode) return StatusCode((int)responseMessage.StatusCode);
            return RedirectToAction("ContactList");


        }

        [HttpGet]
        public async Task<IActionResult> UpdateContact(int id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var responseMessage = await client.GetAsync("api/Contacts/GetContact?id=" + id);
            if (!responseMessage.IsSuccessStatusCode) return StatusCode((int)responseMessage.StatusCode);

            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<UpdateContactDto>(jsonData);
            return View(value);


        }
        [HttpPost]
        public async Task<IActionResult> UpdateContact(UpdateContactDto updateContactDto)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var jsonData = JsonConvert.SerializeObject(updateContactDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("api/Contacts", stringContent);
            if (!responseMessage.IsSuccessStatusCode) { await AddApiErrorsAsync(responseMessage); return View(updateContactDto); }

            return RedirectToAction("ContactList");

        }
    }
}
