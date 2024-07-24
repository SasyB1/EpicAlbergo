using EpicAlbergo.Models;
using EpicAlbergo.Models.Dto;
using Microsoft.Data.SqlClient;
using System.Data;
namespace EpicAlbergo.Services
{
    public class RoomService
    {
        private readonly  IConfiguration _config;

        public RoomService(IConfiguration config)
        {
            _config = config;
        }

        public List<RoomDto> GetAvailableRooms(string roomType)
        {
            var rooms = new List<RoomDto>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    conn.Open();
                    const string SELECT_AVAILABLE_ROOMS = @"
                SELECT RoomId, RoomNumber
                FROM Rooms
                WHERE RoomType = @RoomType";
                    using (SqlCommand cmd = new SqlCommand(SELECT_AVAILABLE_ROOMS, conn))
                    {
                        // Aggiunta del parametro
                        cmd.Parameters.Add(new SqlParameter("@RoomType", SqlDbType.NVarChar) { Value = roomType });

                        // Esecuzione della query
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Lettura dei dati
                            while (reader.Read())
                            {
                                var room = new RoomDto
                                {
                                    RoomId = reader.GetInt32(0),
                                    RoomNumber = reader.GetInt32(1)
                                };

                                rooms.Add(room);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore nel recupero delle camere", ex);
            }

            return rooms;
        }

    }
}
