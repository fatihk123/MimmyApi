using ApiProjeKampi.WebApi.Context;
using ApiProjeKampi.WebApi.Dtos.MessageDtos;
using ApiProjeKampi.WebApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjeKampi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public MessagesController(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult MessageList()
        {
            var value = _context.Messages.ToList();
            return Ok(_mapper.Map<List<ResultMessageDto>>(value));
        }

        [HttpPost]
        public IActionResult CreateMessage(CreateMessageDto createMessageDto)
        {
            createMessageDto.SendDate = DateTime.UtcNow;
            createMessageDto.IsRead = false;
            var value = _mapper.Map<Message>(createMessageDto);
            _context.Messages.Add(value);
            _context.SaveChanges();
            return Ok("Ekleme İşlemi Başarılı");
        }
        [HttpDelete]
        public IActionResult DeleteMessage(int id)
        {
            var value = _context.Messages.Find(id);
            if (value is null) return NotFound();
            _context.Messages.Remove(value);
            _context.SaveChanges();
            return Ok("Silme İşlemi Başarılı");

        }
        [HttpPut]
        public IActionResult UpdateMessage(UpdateMessageDto updateMessageDto)
        {
            var value = _mapper.Map<Message>(updateMessageDto);
            if (!_context.Messages.Any(x => x.MessageId == value.MessageId)) return NotFound();
            _context.Messages.Update(value);
            _context.SaveChanges();
            return Ok("Güncelleme İşlemi Başarılı");
        }
        [HttpGet("GetById")]
        public IActionResult GetByIdMessage(int id)
        {
            var value = _context.Messages.Find(id);
            if (value is null) return NotFound();
            return Ok(_mapper.Map<ResultMessageDto>(value));
        }


        [HttpGet("GetMessageByIsRead")]
        public IActionResult GetMessageByIsRead()
        {
            var value = _context.Messages.Where(x => x.IsRead == false).ToList();
            return Ok(_mapper.Map<List<ResultMessageDto>>(value));
        }

    }

}
