namespace EpicAlbergo.Models
{
    public class ServiceReservation
    {
        public int ReservationServiceId { get; set; }
        public int ReservationId { get; set; }
        public int ServiceId { get; set; }
        public DateTime ServiceDate { get; set; }
        public int ServiceQuantity { get; set; }
        public decimal ServicePrice { get; set; }
    }
}
