namespace EpicAlbergo.Models.Dto
{
    public class CheckoutDto
    {
        public string ReservationNumber { get; set; }
        public DateTime ReservationDate { get; set; }
        public decimal TotalPrice { get; set; }
        public List<Service> Services { get; set; }
    }
}
