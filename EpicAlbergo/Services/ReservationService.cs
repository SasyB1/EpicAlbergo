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
                        "INSERT INTO Reservations (CustomerId, RoomId, ReservationStartStayDate, ReservationEndStayDate, ReservationDeposit, ReservationPrice, ReservationType) " +
                        "VALUES (@CustomerId, @RoomId, @ReservationStartStayDate, @ReservationEndStayDate, @ReservationDeposit, @ReservationPrice, @ReservationType)";

                    using (SqlCommand cmd = new SqlCommand(INSERT_COMMAND, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", reservation.CustomerId);
                        cmd.Parameters.AddWithValue("@RoomId", reservation.RoomId);
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

        public bool IsRoomAvailable(int roomId, DateTime startDate, DateTime endDate)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    conn.Open();
                    const string CHECK_COMMAND = @"
                SELECT COUNT(*)
                FROM Reservations
                WHERE RoomId = @RoomId
                AND @StartDate < ReservationEndStayDate
                AND @EndDate > ReservationStartStayDate";

                    using (SqlCommand cmd = new SqlCommand(CHECK_COMMAND, conn))
                    {
                        cmd.Parameters.AddWithValue("@RoomId", roomId);
                        cmd.Parameters.AddWithValue("@StartDate", startDate);
                        cmd.Parameters.AddWithValue("@EndDate", endDate);

                        int count = (int)cmd.ExecuteScalar();
                        return count == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore nel controllo della disponibilità della camera: " + ex.Message, ex);
            }
        }

        public bool IsCustomerAvailable(int customerId, DateTime startDate, DateTime endDate)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    conn.Open();
                    const string CHECK_COMMAND = @"
                SELECT COUNT(*)
                FROM Reservations
                WHERE CustomerId = @CustomerId
                AND @StartDate < ReservationEndStayDate
                AND @EndDate > ReservationStartStayDate";

                    using (SqlCommand cmd = new SqlCommand(CHECK_COMMAND, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", customerId);
                        cmd.Parameters.AddWithValue("@StartDate", startDate);
                        cmd.Parameters.AddWithValue("@EndDate", endDate);

                        int count = (int)cmd.ExecuteScalar();
                        return count == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore nel controllo della disponibilità del cliente: " + ex.Message, ex);
            }
        }

        public List<Reservation> GetReservationNumber()
        {
            List<Reservation> reservationNumbers = new List<Reservation>();
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
                                Reservation reservation = new Reservation
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


        public void AddServiceToReservation(ServiceReservationDto serviceReservation)
        {
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
                        cmd.Parameters.AddWithValue("@ReservationId", serviceReservation.ReservationId);
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

        public List<Reservation> GetAllReservations()
        {
            List<Reservation> reservations = new List<Reservation>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    conn.Open();
                    const string SELECT_COMMAND = @"
                SELECT ReservationId, CustomerId, RoomId, ReservationNumber, ReservationStartStayDate, ReservationEndStayDate, ReservationDeposit, ReservationPrice, ReservationType
                FROM Reservations";
                    using (SqlCommand cmd = new SqlCommand(SELECT_COMMAND, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Reservation reservation = new Reservation
                                {
                                    ReservationId = reader.GetInt32(0), 
                                    CustomerId = reader.GetInt32(1),
                                    RoomId = reader.GetInt32(2),
                                    ReservationNumber = reader.GetString(3),
                                    ReservationStartStayDate = reader.GetDateTime(4),
                                    ReservationEndStayDate = reader.GetDateTime(5),
                                    ReservationDeposit = reader.GetDecimal(6),
                                    ReservationPrice = reader.GetDecimal(7),
                                    ReservationType = Enum.Parse<ReservationType>(reader.GetString(8))
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

        public List<Service> GetAllServices()
        {
            List<Service> services = new List<Service>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    conn.Open();
                    const string SELECT_COMMAND = "SELECT ServiceId, ServiceType FROM Services";
                    using (SqlCommand cmd = new SqlCommand(SELECT_COMMAND, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Service service = new Service
                                {
                                    ServiceId = reader.GetInt32(0),
                                    ServiceType = reader.GetString(1)
                                };
                                services.Add(service);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore nel recupero dei servizi: " + ex.Message, ex);
            }
            return services;
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
