namespace ApiProjeKampi.WebApi.Entities
{
    public class Notification
    {

        public int NotificationId { get; set; }
        public string Descrption { get; set; }
        public DateTime NotificationDate { get; set; }
        public string IconUrl{ get; set; }
        public bool IsRead{ get; set; }


    }
}
