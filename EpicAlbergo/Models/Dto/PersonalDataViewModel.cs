using System.ComponentModel.DataAnnotations;

namespace EpicAlbergo.Models.Dto
{
    public class PersonalDataViewModel
    {
        [Display(Name = "Nome")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Immettere il nome")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Il nome deve contenere almeno 2 caratteri e non più di 50")]
        public string CustomerName { get; set; }

        [Display(Name = "Cognome")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Immettere il cognome")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Il cognome deve contenere almeno 2 caratteri e non più di 50")]
        public string CustomerSurname { get; set; }

        [Display(Name = "Data di nascita")]
        [Required(ErrorMessage = "Immettere la data di nascita")]
        public DateOnly CustomerBirthday { get; set; }

        [Display(Name = "Sesso")]
        [Required(ErrorMessage = "Selezionare il sesso")]
        public char Gender { get; set; }

        [Display(Name = "Città di nascita")]
        [Required(ErrorMessage = "Immettere la città di nascita")]
        public int CustomerBirthCity { get; set; }

        [Display(Name = "Indirizzo")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Immettere l'indirizzo")]
        [StringLength(50, ErrorMessage = "L'indirizzo non può superare i 50 caratteri")]
        public string CustomerAddress { get; set; }

        [Display(Name = "Città")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Immettere la città")]
        [StringLength(50, ErrorMessage = "La città non può superare i 50 caratteri")]
        public string CustomerCity { get; set; }

        [Display(Name = "CAP")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Immettere il CAP")]
        [StringLength(50, ErrorMessage = "Il CAP non può superare i 50 caratteri")]
        public string CustomerZIPCode { get; set; }

        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Immettere l'email")]
        [EmailAddress(ErrorMessage = "Indirizzo email non valido")]
        [StringLength(50, ErrorMessage = "L'email non può superare i 50 caratteri")]
        public string CustomerEmail { get; set; }

        [Display(Name = "Telefono")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Immettere il telefono")]
        [StringLength(50, ErrorMessage = "Il numero di telefono non può superare i 50 caratteri")]
        public string CustomerTelephone { get; set; }

        public string? CustomerHomeTelephone { get; set; }

        [Required(ErrorMessage = "Codice fiscale obbligatorio")]
        [StringLength(16, ErrorMessage = "Il codice fiscale non può superare i 16 caratteri")]
        public string CustomerTaxIdCode { get; set; }
    }
}
