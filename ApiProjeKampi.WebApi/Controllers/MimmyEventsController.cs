using ApiProjeKampi.WebApi.Context;
using ApiProjeKampi.WebApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjeKampi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MimmyEventsController : ControllerBase
    {
        private readonly ApiContext _context;
        public MimmyEventsController(ApiContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult MimmyEventList()
        {
            var values = _context.MimmyEvents.ToList();
            return Ok(values);
        }
        [HttpPost]
        public IActionResult CreateMimmyEvent(MimmyEvent mimmyEvent)
        {
            _context.MimmyEvents.Add(mimmyEvent);
            _context.SaveChanges();
            return Ok("Etkinlik Ekleme İşlemi Başarılı");
        }
        [HttpDelete]
        public IActionResult DeleteMimmyEvent(int id)
        {
            var value = _context.MimmyEvents.Find(id);
            if (value is null) return NotFound();
            _context.MimmyEvents.Remove(value);
            _context.SaveChanges();
            return Ok("Etkinlik Silme İşlemi Başarılı");
        }
        [HttpPut]
        public IActionResult UpdateMimmyEvent(MimmyEvent mimmyEvent)
        {
            if (!_context.MimmyEvents.Any(x => x.MimmyEventId == mimmyEvent.MimmyEventId)) return NotFound();
            _context.MimmyEvents.Update(mimmyEvent);
            _context.SaveChanges();
            return Ok("Etkinlik Güncelleme İşlemi Başarılı");
        }
        [HttpGet("GetMimmyEvent")]
        public IActionResult GetMimmyEvent(int id)
        {
            var value = _context.MimmyEvents.Find(id);
            if (value is null) return NotFound();
            return Ok(value);
        }





    }
}
