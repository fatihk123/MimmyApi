namespace ApiProjeKampi.WebApi.Dtos.FeatureDtos
{
    public class GetByIdFeatureDto
    {

        public int FeatureId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string SubTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
