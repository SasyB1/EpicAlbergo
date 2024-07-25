using EpicAlbergo.Models.Dto;

namespace EpicAlbergo.Interfaces
{
    public interface ICsvCityService
    {
        IEnumerable<CityDto> GetByProvince(string acronym);
        CityDto GetCityById(int id);
        CityDto GetCityByName(string cityName);
        IEnumerable<ProvinceDto> GetProvinces();
    }
}
