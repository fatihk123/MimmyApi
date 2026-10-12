using ApiProjeKampi.WebApi.Context;
using ApiProjeKampi.WebApi.Dtos.ImageDtos;
using ApiProjeKampi.WebApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjeKampi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {

        private readonly ApiContext _context;
        private readonly IMapper _mapper;
        public ImagesController(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult ImageList()
        {
            var value = _context.Images.ToList();
            return Ok(_mapper.Map<List<ResultImageDto>>(value));
        }
        [HttpPost]
        public IActionResult CreateImage(CreateImageDto createImageDto)
        {
            var value = _mapper.Map<Image>(createImageDto);
            _context.Images.Add(value);
            _context.SaveChanges();
            return Ok("Resim Ekleme İşlemi Başarılı");
        }
        [HttpDelete]
        public IActionResult DeleteImage(int id)
        {
            var value = _context.Images.Find(id);
            if (value is null) return NotFound();
            _context.Images.Remove(value);
            _context.SaveChanges();
            return Ok("Resim Silme İşlemi Başarılı");
        }
        [HttpPut]
        public IActionResult UpdateImage(UpdateImageDto updateImageDto)
        {
            var value = _mapper.Map<Image>(updateImageDto);
            if (!_context.Images.Any(x => x.ImageId == value.ImageId)) return NotFound();
            _context.Images.Update(value);
            _context.SaveChanges();
            return Ok("Resim Güncelleme İşlemi Başarılı");
        }
        [HttpGet("GetImage")]
        public IActionResult GetImage(int id)
        {
            var value = _context.Images.Find(id);
            if (value is null) return NotFound();
            return Ok(value);
        }

    }
}
