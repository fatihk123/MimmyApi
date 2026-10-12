using ApiProjeKampi.WebUI.Dtos.ImageDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ApiProjeKampi.WebUI.Controllers
{
    public class GalleryController : ApiProjeKampi.WebUI.Infrastructure.ApiUiController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GalleryController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<IActionResult> ImageList()
        {
            var client = _httpClientFactory.CreateClient("Api");
            var responseMessage = await client.GetAsync("api/Images");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultImageDto>>(jsonData);
                return View(values);
            }


            return View(new List<ResultImageDto>());
        }
        [HttpGet]
        public IActionResult CreateImage()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> CreateImage(CreateImageDto createImageDto)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var jsonData = JsonConvert.SerializeObject(createImageDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("api/Images", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("ImageList");
            }
            await AddApiErrorsAsync(responseMessage);
            return View(createImageDto);

        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var responseMessage = await client.DeleteAsync("api/Images?id=" + id);
            if (!responseMessage.IsSuccessStatusCode) return StatusCode((int)responseMessage.StatusCode);
            return RedirectToAction("ImageList");


        }

        [HttpGet]
        public async Task<IActionResult> UpdateImage(int id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var responseMessage = await client.GetAsync("api/Images/GetImage?id=" + id);
            if (!responseMessage.IsSuccessStatusCode) return StatusCode((int)responseMessage.StatusCode);

            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<UpdateImageDto>(jsonData);
            return View(value);


        }
        [HttpPost]
        public async Task<IActionResult> UpdateImage(UpdateImageDto updateImageDto)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var jsonData = JsonConvert.SerializeObject(updateImageDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("api/Images", stringContent);
            if (!responseMessage.IsSuccessStatusCode) { await AddApiErrorsAsync(responseMessage); return View(updateImageDto); }

            return RedirectToAction("ImageList");

        }
    }
}
