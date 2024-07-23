using EpicAlbergo.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using EpicAlbergo.Services;
using EpicAlbergo.Models;

namespace EpicAlbergo.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ReservationService _reservationService;

        public ReservationController(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public IActionResult Index()
        {
            return View(_reservationService.GetAllReservations());
        }
        public IActionResult RegisterReservation()
        {
            ViewBag.ReservationTypes = Enum.GetValues(typeof(ReservationType)).Cast<ReservationType>();
            return View();
        }

        [HttpPost]
        public IActionResult RegisterReservation(ReservationDto reservation)
        {
            if (ModelState.IsValid)
            {
                _reservationService.NewReservation(reservation);
                return RedirectToAction("Index");
            }
            return View(reservation);
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
