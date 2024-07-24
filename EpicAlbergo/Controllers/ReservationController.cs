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
            return View(reservation);
        }

        public JsonResult GetAvailableRooms()
        {
            var availableRooms = _roomService.GetAllRooms();
            return Json(availableRooms);
        }
        public IActionResult AssociateService()
        {
            ViewBag.ReservationNumber = _reservationService.GetReservationNumber();
            return View();
        }

        [HttpPost]
        public IActionResult AssociateService(ServiceReservationDto serviceReservation, string reservationNumber)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int reservationId = _reservationService.GetReservationIdByNumber(reservationNumber);
                    serviceReservation.ReservationId = reservationId;
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

        public async Task<IActionResult> Checkout(int reservationId)
            {
            try
            {
                var checkoutDto = await _reservationService.Checkout(reservationId);
                return View("Checkout");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { errorMessage = "Errore durante il checkout: " + ex.Message });
            }
        }
    }
}
