using EpicAlbergo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EpicAlbergo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly CustomerService _customerService;
        private readonly ReservationService _reservationService;

        public ApiController(CustomerService customerService, ReservationService reservationService)
        {
            _customerService = customerService;
            _reservationService = reservationService;
        }

        [HttpGet("SearchByPartialFiscalCode")]
        public IActionResult SearchByPartialFiscalCode(string partialFiscalCode)
        {
            if (string.IsNullOrEmpty(partialFiscalCode))
            {
                return BadRequest(new { success = false, message = "Il codice fiscale è obbligatorio." });
            }
            var customers = _customerService.GetCustomersByPartialFiscalCode(partialFiscalCode);
            if (customers == null || customers.Count == 0)
            {
                return NotFound(new { success = false, message = "Nessun cliente trovato con il codice fiscale fornito." });
            }

            return Ok(new { success = true, data = customers });
        }

        [HttpGet("GetFullBoardReservations")]
        public IActionResult GetFullBoardReservations()
        {
            var reservations = _reservationService.GetFullBoardReservations();
            if (reservations == null || reservations.Count == 0)
            {
                return NotFound(new { success = false, message = "Nessuna prenotazione trovata per FullBoard." });
            }

            return Ok(new { success = true, data = reservations });
        }
    }
}
