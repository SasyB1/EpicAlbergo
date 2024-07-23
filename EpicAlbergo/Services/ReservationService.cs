using EpicAlbergo.Models.Dto;
using Microsoft.Data.SqlClient;
using EpicAlbergo.Models;
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
                        "INSERT INTO Reservations (CustomerId, RoomId, ReservationDate, ReservationStartStayDate, ReservationEndStayDate, ReservationDeposit, ReservationPrice, ReservationType) " +
                        "VALUES (@CustomerId, @RoomId, @ReservationDate, @ReservationStartStayDate, @ReservationEndStayDate, @ReservationDeposit, @ReservationPrice, @ReservationType)";

                    using (SqlCommand cmd = new SqlCommand(INSERT_COMMAND, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", reservation.CustomerId);
                        cmd.Parameters.AddWithValue("@RoomId", reservation.RoomId);
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
                        SELECT CustomerId, RoomId, ReservationNumber, ReservationDate, ReservationStartStayDate, ReservationEndStayDate, ReservationDeposit, ReservationPrice, ReservationType
                        FROM Reservations";
                    using (SqlCommand cmd = new SqlCommand(SELECT_COMMAND, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ReservationDto reservation = new ReservationDto
                                {
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

        public async Task<CheckoutDto> Checkout(int reservationId)
        {
            try
            {
                var checkout = new CheckoutDto
                {
                    Services = new List<Service>()
                };

                using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    const string SELECT_COMMAND = @"
                SELECT 
                    R.ReservationNumber,
                    R.ReservationDate,
                    (R.ReservationPrice - R.ReservationDeposit + COALESCE(SUM(RS.ServicePrice * RS.ServiceQuantity), 0)) AS TotalPrice
                FROM 
                    Reservations AS R
                LEFT JOIN 
                    ReservationsServices AS RS ON R.ReservationId = RS.ReservationId
                LEFT JOIN 
                    Services AS S ON RS.ServiceId = S.ServiceId
                WHERE 
                    R.ReservationId = @ReservationId
                GROUP BY 
                    R.ReservationNumber, 
                    R.ReservationDate, 
                    R.ReservationPrice, 
                    R.ReservationDeposit";

                    using (var cmd = new SqlCommand(SELECT_COMMAND, conn))
                    {
                        cmd.Parameters.AddWithValue("@ReservationId", reservationId);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                checkout.ReservationNumber = reader.GetString(0);
                                checkout.ReservationDate = reader.GetDateTime(1);
                                checkout.TotalPrice = reader.GetDecimal(2);
                            }
                            else
                            {
                                throw new Exception("Prenotazione non trovata.");
                            }
                        }
                    }

                    const string SELECT_SERVICES_COMMAND = @"
                SELECT 
                    S.ServiceType,
                    RS.ServiceQuantity
                FROM 
                    ReservationsServices AS RS
                JOIN 
                    Services AS S ON RS.ServiceId = S.ServiceId
                WHERE 
                    RS.ReservationId = @ReservationId";

                    using (var cmdServices = new SqlCommand(SELECT_SERVICES_COMMAND, conn))
                    {
                        cmdServices.Parameters.AddWithValue("@ReservationId", reservationId);

                        using (var readerServices = await cmdServices.ExecuteReaderAsync())
                        {
                            while (await readerServices.ReadAsync())
                            {
                                var service = new Service
                                {
                                    ServiceType = readerServices.GetString(0)
                                };
                                checkout.Services.Add(service);
                            }
                        }
                    }
                }

                return checkout;
            }
            catch (Exception ex)
            {
                // Opzionalmente, puoi loggare l'eccezione qui
                throw new Exception("Errore nel recupero del checkout: " + ex.Message, ex);
            }
        }

    }
}
