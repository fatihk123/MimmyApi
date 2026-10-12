using System.ComponentModel.DataAnnotations;
namespace ApiProjeKampi.WebUI.Dtos.MessageDtos
{
    public class CreateMessageDto
    {
        [Required, StringLength(100)]
        public string? NameSurname { get; set; }
        [Required, EmailAddress, StringLength(254)]
        public string? Email { get; set; }
        [Required, StringLength(150)]
        public string? Subject { get; set; }
        [Required, StringLength(4000)]
        public string? MessageDetails { get; set; }
        public DateTime SendDate { get; set; }
        public bool IsRead { get; set; }
    }
}
