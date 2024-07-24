using System.ComponentModel.DataAnnotations;

namespace EpicAlbergo.Models.Dto
{
    public enum ReservationType
    {
        OvernightWithBreakfast,
        FullBoard,
        HalfBoard
    }
    public class ReservationDto
    {
        [Required(ErrorMessage = "Il campo  è obbligatorio.")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Il campo  è obbligatorio.")]
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Il campo ReservationStartStayDate è obbligatorio.")]
        [DataType(DataType.Date, ErrorMessage = "La data di inizio soggiorno deve essere una data valida.")]
        public DateTime ReservationStartStayDate { get; set; }

        [Required(ErrorMessage = "Il campo ReservationEndStayDate è obbligatorio.")]
        [DataType(DataType.Date, ErrorMessage = "La data di fine soggiorno deve essere una data valida.")]
        public DateTime ReservationEndStayDate { get; set; }

        [Required(ErrorMessage = "Il campo ReservationDeposit è obbligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Il deposito deve essere maggiore di zero.")]
        public decimal ReservationDeposit { get; set; }

        [Required(ErrorMessage = "Il campo ReservationPrice è obbligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Il prezzo deve essere maggiore di zero.")]
        public decimal ReservationPrice { get; set; }

        [Required(ErrorMessage = "Il campo ReservationType è obbligatorio.")]
        public ReservationType ReservationType { get; set; }
    }
}
