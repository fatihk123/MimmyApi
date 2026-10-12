using ApiProjeKampi.WebUI.Dtos.TestimonialDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiProjeKampi.WebUI.ViewComponents
{
    public class _TestimonialDefaultComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _TestimonialDefaultComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7192/api/Testimonials");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = responseMessage.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<ResultTestimonialDto>>(jsonData.Result);
                return View(result);
            }
            return View();
        }
    }
}
