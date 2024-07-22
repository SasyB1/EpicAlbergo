using System.ComponentModel.DataAnnotations;
using EpicAlbergo.Models;

namespace EpicAlbergo.Models.Dto
{
    public class AssociateServiceDto
    {
        public string ReservationNumber { get; set; }
        public List<Service> Services { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        public DateTime ServiceDate { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La quantità deve essere almeno 1.")]
        public int ServiceQuantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Il prezzo deve essere maggiore di 0.")]
        public decimal ServicePrice { get; set; }
    }
}
