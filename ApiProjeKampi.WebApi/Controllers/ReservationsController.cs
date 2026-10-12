using ApiProjeKampi.WebApi.Context;
using ApiProjeKampi.WebApi.Dtos.ReservationDtos;
using ApiProjeKampi.WebApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjeKampi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ApiContext _context;

        public ReservationsController(IMapper mapper, ApiContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public IActionResult ReservationsList()
        {
            var values = _context.Reservations.ToList();
            return Ok(values);
        }

        [HttpPost]
        public IActionResult CreateReservation(CreateReservationDto createReservationDto)
        {
            if (createReservationDto.ReservationDate.Date < DateTime.Today)
                return BadRequest("Geçmiş bir tarihe rezervasyon yapılamaz.");
            var value = _mapper.Map<Reservation>(createReservationDto);
            value.Message ??= string.Empty;
            _context.Reservations.Add(value);
            _context.SaveChanges();
            return Ok(value);
        }
        [HttpDelete]
        public IActionResult DeleteReservation(int id)
        {
            var value = _context.Reservations.Find(id);
            if (value is null) return NotFound();

            _context.Reservations.Remove(value);
            _context.SaveChanges();
            return Ok("Silme İşlemi Başarılı");
        }

        [HttpPut]
        public IActionResult UpdateReservation(UpdateReservationDto updateReservationDto)
        {
            var value = _mapper.Map<Reservation>(updateReservationDto);
            if (!_context.Reservations.Any(x => x.ReservationID == value.ReservationID)) return NotFound();
            _context.Reservations.Update(value);
            _context.SaveChanges();
            return Ok("Güncelleme İşlemi başarılı");
        }
        [HttpGet("GetById")]
        public IActionResult GetByIdReservation(int id)
        {
            var value = _context.Reservations.Find(id);
            if (value is null) return NotFound();
            return Ok(value);
        }




    }
}
