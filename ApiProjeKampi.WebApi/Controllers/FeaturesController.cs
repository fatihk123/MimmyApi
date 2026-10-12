using ApiProjeKampi.WebApi.Context;
using ApiProjeKampi.WebApi.Dtos.FeatureDtos;
using ApiProjeKampi.WebApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjeKampi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeaturesController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly ApiContext _context;
        public FeaturesController(IMapper mapper, ApiContext context)
        {
            _mapper = mapper;
            _context = context;
        }


        [HttpGet]
        public IActionResult FeatureList()
        {
            var value = _context.Features.ToList();
            return Ok(_mapper.Map<List<ResultFeatureDto>>(value));



        }
        [HttpPost]
        public IActionResult CreateFeature(CreateFeatureDto createFeatureDto)
        {
            var value = _mapper.Map<Feature>(createFeatureDto);
            _context.Features.Add(value);
            _context.SaveChanges();
            return Ok("Özellik Ekleme İşlemi Başarılı");

        }
        [HttpDelete]
        public IActionResult DeleteFeature(int id)
        {
            var value = _context.Features.Find(id);
            if (value is null) return NotFound();
            _context.Features.Remove(value);
            _context.SaveChanges();
            return Ok();
        }
        [HttpPut]
        public IActionResult UpdateFeature(UpdateFeatureDto updateFeatureDto)
        {
            var value = _mapper.Map<Feature>(updateFeatureDto);
            if (!_context.Features.Any(x => x.FeatureId == value.FeatureId)) return NotFound();
            _context.Features.Update(value);
            _context.SaveChanges();
            return Ok("Güncelleme İşlemi Başarılı");
        }

        [HttpGet("GetFeature")]

        public IActionResult GetFeature(int id)
        {
            var value = _context.Features.Find(id);
            if (value is null) return NotFound();
            return Ok(_mapper.Map<ResultFeatureDto>(value));
        }
    }
}
