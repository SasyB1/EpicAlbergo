namespace EpicAlbergo.Models.Dto
{
    public class FullBoardDto
    {
        public int ReservationId { get; set; }
        public int CustomerId { get; set; }
        public int RoomId { get; set; }
        public string ReservationNumber { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime ReservationStartStayDate { get; set; }
        public DateTime ReservationEndStayDate { get; set; }
        public decimal ReservationDeposit { get; set; }
        public decimal ReservationPrice { get; set; }
        public ReservationType ReservationType { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public int RoomNumber { get; set; }
        public string RoomDescription { get; set; }
    }
}
