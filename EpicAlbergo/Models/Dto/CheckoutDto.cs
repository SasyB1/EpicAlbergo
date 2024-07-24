namespace EpicAlbergo.Models.Dto
{
    public class CheckoutDto
    {
        public Reservation Reservation { get; set; }
        public Room Room { get; set; }
        public List<ServiceAllCheckoutDto> ReservationServices { get; set; }
        public decimal TotalPrice { get; set; }
    }
}