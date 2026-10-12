namespace ApiProjeKampi.WebUI.Dtos.TestimonialDtos
{
    public class GetTestimonialByIdDto
    {
        public int TestimonialId { get; set; }
        public string NameSurName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
