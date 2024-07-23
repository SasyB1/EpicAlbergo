namespace EpicAlbergo.Models.Dto
{
    public class CityDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string CadastralCode { get; set; }
        public required ProvinceDto Province { get; set; }
    }
}
