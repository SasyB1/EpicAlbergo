namespace EpicAlbergo.Models
{
    public class Customer
    {
        public enum GenderType
        {
            M,F
        }
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
        public DateTime CustomerBirthday { get; set; }
        public GenderType Gender { get; set; }

    }
}
