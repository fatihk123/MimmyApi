using System.ComponentModel.DataAnnotations;
namespace ApiProjeKampi.WebApi.Dtos.MessageDtos
{
    public class CreateMessageDto
    {

        [Required, StringLength(100)]
        public string NameSurname { get; set; } = string.Empty;
        [Required, EmailAddress, StringLength(254)]
        public string Email { get; set; } = string.Empty;
        [Required, StringLength(150)]
        public string Subject { get; set; } = string.Empty;
        [Required, StringLength(4000)]
        public string MessageDetails { get; set; } = string.Empty;
        public DateTime SendDate { get; set; }
        public bool IsRead { get; set; }
    }
}
