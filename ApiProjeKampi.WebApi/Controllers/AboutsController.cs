using ApiProjeKampi.WebApi.Context;
using ApiProjeKampi.WebApi.Dtos.AboutDtos;
using ApiProjeKampi.WebApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjeKampi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutsController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public AboutsController(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var value = _context.Abouts.ToList();
            return Ok(value);
        }

        [HttpPost]
        public IActionResult CreateAbout(CreateAboutDto createAboutDto)
        {
            var value = _mapper.Map<About>(createAboutDto);
            _context.Abouts.Add(value);
            _context.SaveChanges();
            return Ok("Ekleme İşlemi Başarılı");

        }
        [HttpDelete]
        public IActionResult DeleteAbout(int id)
        {
            var value = _context.Abouts.Find(id);
            _context.Abouts.Remove(value);
            _context.SaveChanges();
            return Ok("Silme İşlemi Başarılı");
        }
        [HttpPut]
        public IActionResult UpdateAbout(UpdateAboutDto updateAboutDto)
        {
            var value = _mapper.Map<About>(updateAboutDto);
            _context.Abouts.Update(value);
            _context.SaveChanges();
            return Ok("Güncelleme İşlemi Başarılı");

        }
        [HttpGet("GetAbout")]
        public IActionResult GetAbout(int id)
        {
            var value = _context.Abouts.Find(id);
            return Ok(value);
        }


    }
}
