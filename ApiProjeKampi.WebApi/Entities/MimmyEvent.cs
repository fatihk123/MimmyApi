namespace ApiProjeKampi.WebApi.Entities
{
    public class MimmyEvent
    {


        public int MimmyEventId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool Status { get; set; }
        public decimal Price { get; set; }


    }
}
