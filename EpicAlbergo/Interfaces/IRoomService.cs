using EpicAlbergo.Models.Dto;

namespace EpicAlbergo.Interfaces
{
    public interface IRoomService
    {
        List<RoomDto> GetAllRooms();
        RoomDto GetRoomById(int roomId);
    }
}
