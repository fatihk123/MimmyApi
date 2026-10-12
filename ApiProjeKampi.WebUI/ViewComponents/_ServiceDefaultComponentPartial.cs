using ApiProjeKampi.WebUI.Dtos.ServiceDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiProjeKampi.WebUI.ViewComponents
{
    public class _ServiceDefaultComponentPartial: ViewComponent
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public _ServiceDefaultComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7192/api/Services");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = responseMessage.Content.ReadAsStringAsync();
                var result =JsonConvert.DeserializeObject<List<ResultWhyChooseMimmyDto>>(jsonData.Result);
                return View(result);
            }


            return View();
        }
    }
}
