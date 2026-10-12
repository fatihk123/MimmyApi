namespace ApiProjeKampi.WebApi.Dtos.CategoryDtos
{
    public class CreateContactDto
    {
        public string MapLocation { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string OpenHours { get; set; } = string.Empty;
    }
}
