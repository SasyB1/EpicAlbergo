using EpicAlbergo.Models.Dto;
using Microsoft.Data.SqlClient;
using EpicAlbergo.Models;
using EpicAlbergo.Interfaces;

namespace EpicAlbergo.Services
{
    public class ReservationService : IReservationService
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

        public bool IsServiceAlreadyAssociated(int reservationId, int serviceId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    conn.Open();
                    const string CHECK_COMMAND = @"
                SELECT COUNT(*)
                FROM ReservationsServices
                WHERE ReservationId = @ReservationId
                AND ServiceId = @ServiceId";

                    using (SqlCommand cmd = new SqlCommand(CHECK_COMMAND, conn))
                    {
                        cmd.Parameters.AddWithValue("@ReservationId", reservationId);
                        cmd.Parameters.AddWithValue("@ServiceId", serviceId);

                        int count = (int)cmd.ExecuteScalar();
                        return count > 0; 
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore nel controllo della presenza del servizio: " + ex.Message, ex);
            }
        }
        public async Task<CheckoutDto> GetCheckout(int reservationId)
        {
            var model = new CheckoutDto
            {
                ReservationServices = new List<ServiceAllCheckoutDto>()  
            };

            var connectionString = _config.GetConnectionString("DefaultConnection");

            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var reservationQuery = @"
            SELECT r.*, rm.RoomNumber 
            FROM Reservations r 
            JOIN Rooms rm ON r.RoomId = rm.RoomId 
            WHERE r.ReservationId = @ReservationId";

                using (var cmd = new SqlCommand(reservationQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ReservationId", reservationId);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            model.Reservation = new Reservation
                            {
                                ReservationId = reader.GetInt32(reader.GetOrdinal("ReservationId")),
                                ReservationNumber = reader.GetString(reader.GetOrdinal("ReservationNumber")),
                                ReservationDate = reader.GetDateTime(reader.GetOrdinal("ReservationDate")),
                                ReservationStartStayDate = reader.GetDateTime(reader.GetOrdinal("ReservationStartStayDate")),
                                ReservationEndStayDate = reader.GetDateTime(reader.GetOrdinal("ReservationEndStayDate")),
                                ReservationDeposit = reader.GetDecimal(reader.GetOrdinal("ReservationDeposit")),
                                ReservationPrice = reader.GetDecimal(reader.GetOrdinal("ReservationPrice")),
                                RoomId = reader.GetInt32(reader.GetOrdinal("RoomId"))
                            };

                            model.Room = new Room
                            {
                                RoomId = reader.GetInt32(reader.GetOrdinal("RoomId")),
                                RoomNumber = reader.GetInt32(reader.GetOrdinal("RoomNumber"))
                            };
                        }
                    }
                }
                var serviceQuery = @"
            SELECT 
                rs.ServiceId, 
                rs.ServiceDate, 
                rs.ServiceQuantity, 
                rs.ServicePrice, 
                s.ServiceType 
            FROM 
                ReservationsServices rs 
            INNER JOIN 
                Services s ON rs.ServiceId = s.ServiceId 
            WHERE 
                rs.ReservationId = @ReservationId";

                using (var cmd = new SqlCommand(serviceQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ReservationId", reservationId);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        decimal totalPrice = model.Reservation.ReservationPrice;

                        while (await reader.ReadAsync())
                        {
                            var dto = new ServiceAllCheckoutDto
                            {
                                ReservationId = reservationId,
                                ServiceId = reader.GetInt32(reader.GetOrdinal("ServiceId")),
                                ServiceDate = reader.GetDateTime(reader.GetOrdinal("ServiceDate")),
                                ServiceQuantity = reader.GetInt32(reader.GetOrdinal("ServiceQuantity")),
                                ServicePrice = reader.GetDecimal(reader.GetOrdinal("ServicePrice")),
                                ServiceType = reader.GetString(reader.GetOrdinal("ServiceType"))
                            };

                            model.ReservationServices.Add(dto);
                            totalPrice += dto.ServiceQuantity * dto.ServicePrice;
                        }

                        model.TotalPrice = totalPrice - model.Reservation.ReservationDeposit;
                    }
                }
            }

            return model;
        }


        public List<FullBoardDto> GetFullBoardReservations()
        {
            var reservations = new List<FullBoardDto>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    conn.Open();
                    const string query = @"
                SELECT 
                    r.ReservationId,
                    r.CustomerId,
                    r.RoomId,
                    r.ReservationNumber,
                    r.ReservationDate,
                    r.ReservationStartStayDate,
                    r.ReservationEndStayDate,
                    r.ReservationDeposit,
                    r.ReservationPrice,
                    r.ReservationType,
                    c.CustomerName,
                    c.CustomerSurname,
                    ro.RoomNumber,
                    ro.RoomDescription
                FROM Reservations r
                INNER JOIN Customers c ON r.CustomerId = c.CustomerId
                INNER JOIN Rooms ro ON r.RoomId = ro.RoomId
                WHERE r.ReservationType = @ReservationType";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ReservationType", "FullBoard");
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var reservation = new FullBoardDto
                                {
                                    ReservationId = reader.GetInt32(reader.GetOrdinal("ReservationId")),
                                    CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                    RoomId = reader.GetInt32(reader.GetOrdinal("RoomId")),
                                    ReservationNumber = reader.GetString(reader.GetOrdinal("ReservationNumber")),
                                    ReservationDate = reader.GetDateTime(reader.GetOrdinal("ReservationDate")),
                                    ReservationStartStayDate = reader.GetDateTime(reader.GetOrdinal("ReservationStartStayDate")),
                                    ReservationEndStayDate = reader.GetDateTime(reader.GetOrdinal("ReservationEndStayDate")),
                                    ReservationDeposit = reader.GetDecimal(reader.GetOrdinal("ReservationDeposit")),
                                    ReservationPrice = reader.GetDecimal(reader.GetOrdinal("ReservationPrice")),
                                    ReservationType = Enum.Parse<ReservationType>(reader.GetString(reader.GetOrdinal("ReservationType"))),
                                    CustomerName = reader.GetString(reader.GetOrdinal("CustomerName")),
                                    CustomerSurname = reader.GetString(reader.GetOrdinal("CustomerSurname")),
                                    RoomNumber = reader.GetInt32(reader.GetOrdinal("RoomNumber")),
                                    RoomDescription = reader.GetString(reader.GetOrdinal("RoomDescription"))
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
