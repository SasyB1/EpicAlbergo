using System.ComponentModel.DataAnnotations;

namespace EpicAlbergo.Models.Dto
{
    public class FiscalCodeViewModel
    {
        [Display(Name = "Nome")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Immettere il nome")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Il nome deve contenere almeno 2 caratteri e non più di 20")]
        public required string FirstName { get; set; }

        [Display(Name = "Cognome")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Immettere il cognome")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Il cognome deve contenere almeno 2 caratteri e non più di 20")]
        public required string LastName { get; set; }

        [Display(Name = "Data di nascita")]
        [Required(ErrorMessage = "Immettere la data di nascita")]
        public DateOnly Birthday { get; set; }

        [Display(Name = "Sesso")]
        public char Gender { get; set; }

        [Display(Name = "Città di nascita")]
        [Required(ErrorMessage = "Immettere la città di nascita")]
        public required int BirthOfCity { get; set; }
    }
}
