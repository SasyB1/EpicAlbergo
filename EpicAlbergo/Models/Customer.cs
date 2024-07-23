using EpicAlbergo.Models.Dto;
namespace EpicAlbergo.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerSurname { get; set; }
        public string CustomerName { get; set; }
        public string CustomerBirthCity { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerZIPCode { get; set; }
        public string CustomerEmail { get; set; }
        public string? CustomerHomeTelephone { get; set; }
        public string CustomerTelephone { get; set; }
        public string CustomerTaxIdCode { get; set; }
        public DateOnly CustomerBirthday { get; set; }
        public char Gender { get; set; }

    }
}
