using EpicAlbergo.Models;
using EpicAlbergo.Models.Dto;
using Microsoft.Data.SqlClient;
using System.Data;
using EpicAlbergo.Interfaces;
namespace EpicAlbergo.Services
{
    public class RoomService : IRoomService
    {
        private readonly  IConfiguration _config;

        public RoomService(IConfiguration config)
        {
            _config = config;
        }

        public List<RoomDto> GetAllRooms()
        {
            var rooms = new List<RoomDto>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    conn.Open();
                    const string SELECT_ALL_ROOMS = @"
            SELECT RoomId, RoomNumber, RoomDescription, RoomPrice
            FROM Rooms";

                    using (SqlCommand cmd = new SqlCommand(SELECT_ALL_ROOMS, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var room = new RoomDto
                                {
                                    RoomId = reader.GetInt32(0),
                                    RoomNumber = reader.GetInt32(1),
                                    RoomDescription = reader.GetString(2),
                                    RoomPrice = reader.GetDecimal(3)
                                };

                                rooms.Add(room);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore nel recupero delle stanze", ex);
            }

            return rooms;
        }

        public RoomDto GetRoomById(int roomId)
        {
            RoomDto room = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    conn.Open();
                    const string SELECT_ROOM_BY_ID = @"
                SELECT RoomId, RoomNumber, RoomDescription, RoomPrice
                FROM Rooms
                WHERE RoomId = @RoomId";
                    using (SqlCommand cmd = new SqlCommand(SELECT_ROOM_BY_ID, conn))
                    {
                        cmd.Parameters.AddWithValue("@RoomId", roomId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                room = new RoomDto
                                {
                                    RoomId = reader.GetInt32(0),
                                    RoomNumber = reader.GetInt32(1),
                                    RoomDescription = reader.GetString(2),
                                    RoomPrice = reader.GetDecimal(3)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore nel recupero della camera", ex);
            }
            return room;
        }
    }
}
