using EpicAlbergo.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using EpicAlbergo.Services;
using EpicAlbergo.Models;

namespace EpicAlbergo.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ReservationService _reservationService;
        private readonly CustomerService _customerService;
        private readonly RoomService _roomService;


        public ReservationController(ReservationService reservationService, CustomerService customerService, RoomService roomService)
        {
            _reservationService = reservationService;
            _customerService = customerService;
            _roomService = roomService;
        }

        public IActionResult Index()
        {
            
            return View(_reservationService.GetAllReservations());
        }
        public IActionResult RegisterReservation()
        {
            ViewBag.ReservationTypes = Enum.GetValues(typeof(ReservationType)).Cast<ReservationType>();
            ViewBag.Customers = _customerService.GetCustomers();
            ViewBag.RoomTypes = new List<string> { "Double", "Single" };
            return View();
        }

        [HttpPost]
        public IActionResult RegisterReservation(ReservationDto reservation)
        {
            if (ModelState.IsValid)
            {
                if (!_reservationService.IsRoomAvailable(reservation.RoomId, reservation.ReservationStartStayDate, reservation.ReservationEndStayDate))
                {
                    ModelState.AddModelError("", "La camera selezionata è già occupata nelle date specificate.");
                }
                else if (!_reservationService.IsCustomerAvailable(reservation.CustomerId, reservation.ReservationStartStayDate, reservation.ReservationEndStayDate))
                {
                    ModelState.AddModelError("", "Il cliente ha già una prenotazione nelle date specificate.");
                }
                else
                {
                    try
                    {
                        var room = _roomService.GetRoomById(reservation.RoomId);
                        var roomPrice = room.RoomPrice;
                        _reservationService.NewReservation(reservation);
                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", $"Errore nel salvataggio della prenotazione: {ex.Message}");
                    }
                }
            }
            ViewBag.ReservationTypes = Enum.GetValues(typeof(ReservationType)).Cast<ReservationType>();
            ViewBag.Customers = _customerService.GetCustomers();
            ViewBag.Rooms = _roomService.GetAllRooms();

            return View(reservation);
        }

        public JsonResult GetAllRooms()
        {
            var allRooms = _roomService.GetAllRooms();
            return Json(allRooms);
        }


        public JsonResult CheckRoomAvailability(int roomId, DateTime startDate, DateTime endDate)
        {
            var isAvailable = _reservationService.IsRoomAvailable(roomId, startDate, endDate);
            return Json(isAvailable);
        }


        public JsonResult CheckCustomerAvailability(int customerId, DateTime startDate, DateTime endDate)
        {
            var isAvailable = _reservationService.IsCustomerAvailable(customerId, startDate, endDate);
            return Json(isAvailable);
        }
        public IActionResult AssociateService(int reservationId)
        {
            ViewBag.Services = _reservationService.GetAllServices();

            var model = new ServiceReservationDto
            {
                ReservationId = reservationId,
                ServiceDate = DateTime.Now
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult AssociateService(ServiceReservationDto serviceReservation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _reservationService.AddServiceToReservation(serviceReservation);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(serviceReservation);
        }

        public JsonResult IsServiceAlreadyAssociated(int reservationId, int serviceId)
        {
            bool isAssociated = _reservationService.IsServiceAlreadyAssociated(reservationId, serviceId);
            return Json(!isAssociated); 
        }

        public async Task<IActionResult> Checkout(int reservationId)
        {
            var checkoutDto = await _reservationService.GetCheckoutViewModelAsync(reservationId);
            return View(checkoutDto);
        }

        public IActionResult SearchBy()
            {
            return View();
        }
    }
}
