namespace EpicAlbergo.Models.Dto
{
    public class ServiceReservationDto
    {
        public int ReservationId { get; set; }
        public int ServiceId { get; set; }
        public DateTime ServiceDate { get; set; }
        public int ServiceQuantity { get; set; }
        public decimal ServicePrice { get; set; }
    }
}
