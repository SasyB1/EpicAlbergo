namespace EpicAlbergo.Models
{
    public enum RoomType
    {
        Double,
        Single
    }

    public class Room
    {
        public int RoomId { get; set; }
        public int RoomNumber { get; set; }
        public string RoomDescription { get; set; }
        public RoomType RoomType { get; set; }
    }
}
