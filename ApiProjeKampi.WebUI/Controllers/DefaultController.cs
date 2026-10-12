using ApiProjeKampi.WebUI.Dtos.MessageDtos;
using ApiProjeKampi.WebUI.Dtos.ReservationDtos;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjeKampi.WebUI.Controllers;

public class DefaultController(IHttpClientFactory httpClientFactory) : Controller
{
    public IActionResult Index() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> BookTable(CreateReservationDto reservation)
    {
        if (!ModelState.IsValid) return BadRequest("Lütfen rezervasyon bilgilerini kontrol edin.");
        reservation.ReservationStatus = "Beklemede";
        var response = await httpClientFactory.CreateClient("Api").PostAsJsonAsync("api/Reservations", reservation);
        if (!response.IsSuccessStatusCode) return BadRequest("Rezervasyon kaydedilemedi. Bilgileri kontrol edip tekrar deneyin.");
        return Content("OK");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> SendMessage(CreateMessageDto message)
    {
        if (!ModelState.IsValid) return BadRequest("Lütfen mesaj bilgilerini kontrol edin.");
        message.SendDate = DateTime.UtcNow;
        message.IsRead = false;
        var response = await httpClientFactory.CreateClient("Api").PostAsJsonAsync("api/Messages", message);
        if (!response.IsSuccessStatusCode) return BadRequest("Mesaj kaydedilemedi. Bilgileri kontrol edip tekrar deneyin.");
        return Content("OK");
    }
}
