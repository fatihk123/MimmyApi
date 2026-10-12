using ApiProjeKampi.WebUI.Dtos.MessageDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ApiProjeKampi.WebUI.Controllers
{
    public class MessageController : ApiProjeKampi.WebUI.Infrastructure.ApiUiController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MessageController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<IActionResult> MessageList()
        {
            var client = _httpClientFactory.CreateClient("Api");
            var responseMessage = await client.GetAsync("api/Messages");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultMessageDto>>(jsonData);
                return View(values);
            }


            return View(new List<ResultMessageDto>());
        }
        [HttpGet]
        public IActionResult CreateMessage()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> CreateMessage(CreateMessageDto createMessageDto)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var jsonData = JsonConvert.SerializeObject(createMessageDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("api/Messages", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("MessageList");
            }
            await AddApiErrorsAsync(responseMessage);
            return View(createMessageDto);

        }

        [HttpPost]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var responseMessage = await client.DeleteAsync("api/Messages?id=" + id);
            if (!responseMessage.IsSuccessStatusCode) return StatusCode((int)responseMessage.StatusCode);
            return RedirectToAction("MessageList");


        }

        [HttpGet]
        public async Task<IActionResult> UpdateMessage(int id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var responseMessage = await client.GetAsync("api/Messages/GetById?id=" + id);
            if (!responseMessage.IsSuccessStatusCode) return StatusCode((int)responseMessage.StatusCode);

            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<UpdateMessageDto>(jsonData);
            return View(value);


        }
        [HttpPost]
        public async Task<IActionResult> UpdateMessage(UpdateMessageDto updateMessageDto)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var jsonData = JsonConvert.SerializeObject(updateMessageDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("api/Messages", stringContent);
            if (!responseMessage.IsSuccessStatusCode) { await AddApiErrorsAsync(responseMessage); return View(updateMessageDto); }

            return RedirectToAction("MessageList");

        }
        [HttpGet]
        public async Task<IActionResult> OpenMessage(int id)
        {
            var response = await _httpClientFactory.CreateClient("Api").GetAsync("api/Messages/GetById?id=" + id);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return NotFound();
            response.EnsureSuccessStatusCode();
            var message = await response.Content.ReadFromJsonAsync<GetMessageByIdDto>();
            return View(message);
        }

        [HttpPost]
        public async Task<IActionResult> AnswerMessageWithOpenAI(int id, [FromServices] ApiProjeKampi.WebUI.Services.OpenAiService ai, CancellationToken cancellationToken)
        {
            var response = await _httpClientFactory.CreateClient("Api").GetAsync("api/Messages/GetById?id=" + id, cancellationToken);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return NotFound();
            response.EnsureSuccessStatusCode();
            var message = await response.Content.ReadFromJsonAsync<GetMessageByIdDto>(cancellationToken: cancellationToken);
            try
            {
                ViewBag.Answer = await ai.GenerateAsync("Mimmy restoranı adına müşterinin mesajına Türkçe, nazik ve kısa bir yanıt taslağı hazırla. Yapılmamış işlemleri yapılmış gibi söyleme ve bilmediğin bilgileri uydurma. HTML üretme.", message!.MessageDetails ?? "", cancellationToken);
            }
            catch (ApiProjeKampi.WebUI.Services.AiServiceException error) { ModelState.AddModelError("", error.Message); }
            return View("OpenMessage", message);
        }
    }
}
