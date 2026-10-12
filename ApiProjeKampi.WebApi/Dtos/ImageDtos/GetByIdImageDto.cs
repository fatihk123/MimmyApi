namespace ApiProjeKampi.WebApi.Dtos.ImageDtos
{
    public class GetByIdImageDto
    {
        public int ImageId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
