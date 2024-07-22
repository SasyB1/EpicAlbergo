using EpicAlbergo.Models.Dto;
using Microsoft.Data.SqlClient;
namespace EpicAlbergo.Services
{
    public class ReservationService
    {
        private readonly IConfiguration _config;

        public ReservationService(IConfiguration config)
        {
            _config = config;
        }

        public void NewReservation(ReservationDto reservation)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    conn.Open();
                    const string INSERT_COMMAND =
                        "INSERT INTO Reservations (CustomerId, RoomId, ReservationNumber, ReservationDate, ReservationStartStayDate, ReservationEndStayDate, ReservationDeposit, ReservationPrice, ReservationType) " +
                        "VALUES (@CustomerId, @RoomId, @ReservationNumber, @ReservationDate, @ReservationStartStayDate, @ReservationEndStayDate, @ReservationDeposit, @ReservationPrice, @ReservationType)";

                    using (SqlCommand cmd = new SqlCommand(INSERT_COMMAND, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", reservation.CustomerId);
                        cmd.Parameters.AddWithValue("@RoomId", reservation.RoomId);
                        cmd.Parameters.AddWithValue("@ReservationNumber", reservation.ReservationNumber);
                        cmd.Parameters.AddWithValue("@ReservationDate", reservation.ReservationDate);
                        cmd.Parameters.AddWithValue("@ReservationStartStayDate", reservation.ReservationStartStayDate);
                        cmd.Parameters.AddWithValue("@ReservationEndStayDate", reservation.ReservationEndStayDate);
                        cmd.Parameters.AddWithValue("@ReservationDeposit", reservation.ReservationDeposit);
                        cmd.Parameters.AddWithValue("@ReservationPrice", reservation.ReservationPrice);
                        cmd.Parameters.AddWithValue("@ReservationType", reservation.ReservationType.ToString());
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore nel salvataggio della prenotazione: " + ex.Message, ex);
            }
        }

        public List<ReservationDto> GetReservationNumber()
        {
            List<ReservationDto> reservationNumbers = new List<ReservationDto>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    conn.Open();
                    const string SELECT_COMMAND = "SELECT ReservationNumber FROM Reservations";
                    using (SqlCommand cmd = new SqlCommand(SELECT_COMMAND, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ReservationDto reservation = new ReservationDto
                                {
                                    ReservationNumber = reader.GetString(0)
                                };
                                reservationNumbers.Add(reservation);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore nel recupero delle prenotazioni: " + ex.Message, ex);
            }
            return reservationNumbers;
        }

        public int GetReservationIdByNumber(string reservationNumber)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    conn.Open();
                    const string SELECT_COMMAND = "SELECT ReservationId FROM Reservations WHERE ReservationNumber = @ReservationNumber";
                    using (SqlCommand cmd = new SqlCommand(SELECT_COMMAND, conn))
                    {
                        cmd.Parameters.AddWithValue("@ReservationNumber", reservationNumber);
                        object result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int reservationId))
                        {
                            return reservationId;
                        }
                        else
                        {
                            throw new Exception("Prenotazione non trovata.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore nel recupero dell'ID della prenotazione: " + ex.Message, ex);
            }
        }

        public void AddServiceToReservation(ServiceReservationDto serviceReservation)
        {
            int reservationId = GetReservationIdByNumber(serviceReservation.ReservationId.ToString());

            try
            {
                using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    conn.Open();
                    const string INSERT_COMMAND = @"
                        INSERT INTO ReservationsServices (ReservationId, ServiceId, ServiceDate, ServiceQuantity, ServicePrice)
                        VALUES (@ReservationId, @ServiceId, @ServiceDate, @ServiceQuantity, @ServicePrice)";

                    using (SqlCommand cmd = new SqlCommand(INSERT_COMMAND, conn))
                    {
                        cmd.Parameters.AddWithValue("@ReservationId", reservationId);
                        cmd.Parameters.AddWithValue("@ServiceId", serviceReservation.ServiceId);
                        cmd.Parameters.AddWithValue("@ServiceDate", serviceReservation.ServiceDate);
                        cmd.Parameters.AddWithValue("@ServiceQuantity", serviceReservation.ServiceQuantity);
                        cmd.Parameters.AddWithValue("@ServicePrice", serviceReservation.ServicePrice);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore nell'aggiunta del servizio alla prenotazione: " + ex.Message, ex);
            }
        }

        public List<ReservationDto> GetAllReservations()
        {
            List<ReservationDto> reservations = new List<ReservationDto>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    conn.Open();
                    const string SELECT_COMMAND = @"
                        SELECT ReservationId, CustomerId, RoomId, ReservationNumber, ReservationDate, ReservationStartStayDate, ReservationEndStayDate, ReservationDeposit, ReservationPrice, ReservationType
                        FROM Reservations";
                    using (SqlCommand cmd = new SqlCommand(SELECT_COMMAND, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ReservationDto reservation = new ReservationDto
                                {
                                    ReservationId = reader.GetInt32(0),
                                    CustomerId = reader.GetInt32(1),
                                    RoomId = reader.GetInt32(2),
                                    ReservationNumber = reader.GetString(3),
                                    ReservationDate = reader.GetDateTime(4),
                                    ReservationStartStayDate = reader.GetDateTime(5),
                                    ReservationEndStayDate = reader.GetDateTime(6),
                                    ReservationDeposit = reader.GetDecimal(7),
                                    ReservationPrice = reader.GetDecimal(8),
                                    ReservationType = Enum.Parse<ReservationType>(reader.GetString(9))
                                };
                                reservations.Add(reservation);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore nel recupero delle prenotazioni: " + ex.Message, ex);
            }
            return reservations;
        }
    }
}
 