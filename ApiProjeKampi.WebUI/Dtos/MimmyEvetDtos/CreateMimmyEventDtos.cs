namespace ApiProjeKampi.WebUI.Dtos.MimmyEvetDtos
{
    public class CreateMimmyEventDtos
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool Status { get; set; }
        public decimal Price { get; set; }
    }
}
