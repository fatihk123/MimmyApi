namespace ApiProjeKampi.WebUI.Dtos.ServiceDtos
{
    public class GetWhyChooseMimmyByIdDto
    {
        public int ServiceId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;

    }
}
