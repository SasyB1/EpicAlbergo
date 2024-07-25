using System.ComponentModel.DataAnnotations;

namespace EpicAlbergo.Models.Dto
{
    public class ServiceReservationDto
    {
        public int ReservationId { get; set; }

        [Display(Name = "Service")]
        public int ServiceId { get; set; }

        [Display(Name = "Service Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime ServiceDate { get; set; }

        [Display(Name = "Quantity")]
        public int ServiceQuantity { get; set; }

        [Display(Name = "Price")]
        public decimal ServicePrice { get; set; }
    }
}
