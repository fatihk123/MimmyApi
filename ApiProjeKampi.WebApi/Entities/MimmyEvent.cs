namespace ApiProjeKampi.WebApi.Entities
{
    public class MimmyEvent
    {


        public int MimmyEventId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool Status { get; set; }
        public decimal Price { get; set; }


    }
}
