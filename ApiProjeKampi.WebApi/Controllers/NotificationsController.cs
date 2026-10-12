using ApiProjeKampi.WebApi.Context;
using ApiProjeKampi.WebApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjeKampi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly ApiContext _context;

        public NotificationsController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult NotificationList()
        {
            var values = _context.Notifications.ToList();
            return Ok(values);
        }
        [HttpPost]
        public IActionResult CreateNotification(Notification notification)
        {
            _context.Notifications.Add(notification);
            _context.SaveChanges();
            return Ok("Bildirim Ekleme İşlemi Başarılı");
        }
        [HttpDelete]
        public IActionResult DeleteNotification(int id)
        {
            var value = _context.Notifications.Find(id);
            _context.Notifications.Remove(value);
            _context.SaveChanges();
            return Ok("Bildirim Silme İşlemi Başarılı");
        }
        [HttpPut]
        public IActionResult UpdateNotification(Notification notification)
        {
            _context.Notifications.Update(notification);
            _context.SaveChanges();
            return Ok("Bildirim Güncelleme İşlemi Başarılı");
        }
        [HttpGet("GetNotification")]
        public IActionResult GetNotification(int id)
        {
            var value = _context.Categories.Find(id);
            return Ok(value);
        }

        [HttpGet("GetNotificationByIsReadFalse")]
        public IActionResult GetNotificationByIsReadFalse()
        {
            var values = _context.Notifications.Where(x => x.IsRead == false).ToList();
            return Ok(values);
        }



    }
}
