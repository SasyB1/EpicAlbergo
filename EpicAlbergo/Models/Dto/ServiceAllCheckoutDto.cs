namespace EpicAlbergo.Models.Dto
{
    public class ServiceAllCheckoutDto
    {
        public int ReservationId { get; set; }
        public int ServiceId { get; set; }
        public DateTime ServiceDate { get; set; }
        public int ServiceQuantity { get; set; }
        public decimal ServicePrice { get; set; }
        public string ServiceType { get; set; }
    }
}
