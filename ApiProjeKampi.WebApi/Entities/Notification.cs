namespace ApiProjeKampi.WebApi.Entities
{
    public class Notification
    {

        public int NotificationId { get; set; }
        public string Descrption { get; set; } = string.Empty;
        public DateTime NotificationDate { get; set; }
        public string IconUrl { get; set; } = string.Empty;
        public bool IsRead { get; set; }


    }
}
