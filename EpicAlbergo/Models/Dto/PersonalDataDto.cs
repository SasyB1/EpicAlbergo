namespace EpicAlbergo.Models.Dto
{
    public enum Gender
    {
        Male = 0,
        Female = 40
    }
    public class PersonalDataDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateOnly Birthday { get; set; }
        public Gender Gender { get; set; }
        public required CityDto BirthCity { get; set; }
    }
}
