using EpicAlbergo.Models.Dto;

namespace EpicAlbergo.Interfaces
{
    public interface IFiscalCodeService
    {
        string GenerateFiscalCode(PersonalDataDto data);
        bool ValidateFiscalCode(string fiscalCode);
    }
}
