using ApiProjeKampi.WebUI.Dtos.NotificationDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ApiProjeKampi.WebUI.ViewComponents.AdminLayoutNavbarViewComponent
{
    public class _NavbarNotificationComponentPartial : ViewComponent
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public _NavbarNotificationComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient("Api");
            var responseMessage = await client.GetAsync("api/Notifications/GetNotificationByIsReadFalse");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = responseMessage.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<ResultNotificationDto>>(jsonData.Result);
                return View(result);
            }
            return View(new List<ResultNotificationDto>());
        }





    }
}
