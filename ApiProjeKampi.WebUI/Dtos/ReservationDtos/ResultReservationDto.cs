namespace ApiProjeKampi.WebUI.Dtos.ReservationDtos
{
    public class ResultReservationDto
    {
        public int ReservationID { get; set; }
        public string NameSurname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime ReservationDate { get; set; }
        public string ReservationTime { get; set; } = string.Empty;
        public int CountofPeople { get; set; }
        public string Message { get; set; } = string.Empty;
        public string ReservationStatus { get; set; } = string.Empty;
    }
}
