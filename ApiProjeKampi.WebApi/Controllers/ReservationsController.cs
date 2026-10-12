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
            var value = _mapper.Map<Reservation>(createReservationDto);
            _context.Reservations.Add(value);
            _context.SaveChanges();
            return Ok(value);
        }
        [HttpDelete]
        public IActionResult DeleteReservation(int id)
        {
            var value = _context.Reservations.Find(id);

            _context.Reservations.Remove(value);
            _context.SaveChanges();
            return Ok("Silme İşlemi Başarılı");
        }

        [HttpPut]
        public IActionResult UpdateReservation(UpdateReservationDto updateReservationDto)
        {
            var value = _mapper.Map<Reservation>(updateReservationDto);
            _context.Reservations.Update(value);
            _context.SaveChanges();
            return Ok("Güncelleme İşlemi başarılı");
        }
        [HttpGet("GetById")]
        public IActionResult GetByIdReservation(int id)
        {
            var value = _context.Reservations.Find(id);
            return Ok(value);
        }




    }
}