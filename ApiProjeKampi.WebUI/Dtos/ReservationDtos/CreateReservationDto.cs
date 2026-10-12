using System.ComponentModel.DataAnnotations;
namespace ApiProjeKampi.WebUI.Dtos.ReservationDtos
{
    public class CreateReservationDto
    {
        [Required, StringLength(100)]
        public string NameSurname { get; set; } = string.Empty;
        [Required, EmailAddress, StringLength(254)]
        public string Email { get; set; } = string.Empty;
        [Required, StringLength(30)]
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime ReservationDate { get; set; }
        [Required, RegularExpression(@"^([01]\d|2[0-3]):[0-5]\d(:[0-5]\d)?$")]
        public string ReservationTime { get; set; } = string.Empty;
        [Range(1, 50)]
        public int CountofPeople { get; set; }
        [StringLength(4000)]
        public string? Message { get; set; } = string.Empty;
        public string ReservationStatus { get; set; } = "Beklemede";
    }
}
