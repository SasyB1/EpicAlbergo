namespace EpicAlbergo.Models.Dto
{
    public class ProvinceDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Acronym { get; set; }

        public override bool Equals(object? obj) => obj is ProvinceDto && obj.GetHashCode() == GetHashCode();
        public override int GetHashCode() => HashCode.Combine(Acronym);
    }
}
