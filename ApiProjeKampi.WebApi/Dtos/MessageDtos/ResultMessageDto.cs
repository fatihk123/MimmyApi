namespace ApiProjeKampi.WebApi.Dtos.MessageDtos
{
    public class ResultMessageDto
    {

        public int MessageId { get; set; }
        public string NameSurname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string MessageDetails { get; set; } = string.Empty;
        public DateTime SendDate { get; set; }
        public bool IsRead { get; set; }
    }
}
