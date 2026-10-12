namespace ApiProjeKampi.WebUI.Dtos.ChefDtos
{
    public class UpdateChefDto
    {
        public int ChefId { get; set; }
        public string NameSurName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
