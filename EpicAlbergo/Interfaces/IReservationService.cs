using EpicAlbergo.Models.Dto;
using EpicAlbergo.Models;

namespace EpicAlbergo.Interfaces
{
    public interface IReservationService
    {
        void NewReservation(ReservationDto reservation);
        bool IsRoomAvailable(int roomId, DateTime startDate, DateTime endDate);
        bool IsCustomerAvailable(int customerId, DateTime startDate, DateTime endDate);
        List<Reservation> GetReservationNumber();
        void AddServiceToReservation(ServiceReservationDto serviceReservation);
        List<Reservation> GetAllReservations();
        List<Service> GetAllServices();
        bool IsServiceAlreadyAssociated(int reservationId, int serviceId);
        Task<CheckoutDto> GetCheckout(int reservationId);
        List<FullBoardDto> GetFullBoardReservations();
    }
}
