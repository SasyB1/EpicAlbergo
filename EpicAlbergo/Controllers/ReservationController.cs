using EpicAlbergo.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using EpicAlbergo.Services;

namespace EpicAlbergo.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ReservationService _reservationService;

        public ReservationController(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }


        public IActionResult RegisterReservation()
        {
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

    }
}
