namespace ApiProjeKampi.WebUI.Dtos.AboutDtos
{
    public class GetAboutByIdDto
    {
        public int AboutId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string VideoCoverUrl { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public string Descrption { get; set; } = string.Empty;

        public string ReservationNumber { get; set; } = string.Empty;
    }
}
