using System.Text;
using EpicAlbergo.Models.Dto;
using EpicAlbergo.Interfaces;

namespace EpicAlbergo.Services
{
    public class FiscalCodeService : IFiscalCodeService
    {
        public string GenerateFiscalCode(PersonalDataDto data)
        {
            var fc = new StringBuilder()
                .Append(HandleLastName(data.LastName))
                .Append(HandleFirstName(data.FirstName))
                .Append(HandleBirthday(data.Birthday, data.Gender))
                .Append(HandleBirthCity(data.BirthCity));
            return fc.Append(CalculateCheckCode(fc)).ToString();
        }

        class ConsonantVowels
        {
            public readonly StringBuilder consonants = new StringBuilder();
            public readonly StringBuilder vowels = new StringBuilder();

            public ConsonantVowels(string text)
            {
                text.ToUpper().ToCharArray().ToList().ForEach(c => {
                    if (char.IsLetter(c)) // prende solo le lettere
                        if (c == 'A' || c == 'E' || c == 'I' || c == 'O' || c == 'U')
                            vowels.Append(c);
                        else
                            consonants.Append(c);
                });
            }
        }

        private string HandleLastName(string lastName)
        {
            var cv = new ConsonantVowels(lastName);
            return $"{cv.consonants}{cv.vowels}XXX"[..3];
        }

        private string HandleFirstName(string firstName)
        {
            var cv = new ConsonantVowels(firstName);
            if (cv.consonants.Length > 3) cv.consonants.Remove(1, 1);
            return $"{cv.consonants}{cv.vowels}XXX"[..3];
        }

        private string HandleBirthday(DateOnly birthday, Gender gender)
        {
            const string months = "ABCDEHLMPRST";
            return $"{birthday:yy}{months[birthday.Month - 1]}{birthday.Day + (int)gender}";
        }

        private string HandleBirthCity(CityDto city) { return city.CadastralCode; }
        private char CalculateCheckCode(StringBuilder fc)
        {
            var odds = new int[] { 1, 0, 5, 7, 9, 13, 15, 17, 19, 21, 2, 4, 18, 20, 11, 3, 6, 8, 12, 14, 16, 10, 22, 25, 24, 23 };
            var sum = 0;
            for (int i = 0; i < 15; ++i)
            {
                int depl = char.IsDigit(fc[i]) ? fc[i] - '0' : fc[i] - 'A';
                sum += i % 2 == 0 ? odds[depl] : depl;
            }
            return (char)(sum % 26 + 'A');
        }

        public bool ValidateFiscalCode(string fiscalCode)
        {
            if (fiscalCode.Length != 16) return false;
            var check = CalculateCheckCode(new StringBuilder(fiscalCode[..15]));
            return fiscalCode[15] == check;
        }
    }
}
