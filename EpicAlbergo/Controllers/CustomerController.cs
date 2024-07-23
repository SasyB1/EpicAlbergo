using Microsoft.AspNetCore.Mvc;
using EpicAlbergo.Services;
using EpicAlbergo.Models.Dto;
using EpicAlbergo.Models;


namespace EpicAlbergo.Controllers
{
    public class CustomerController : Controller
    {
        private readonly FiscalCodeService _fiscalCodeService;
        private readonly CsvCityService _cityService;
        private readonly CustomerService _customerService;

        public CustomerController(FiscalCodeService fiscalCodeService, CsvCityService csvCityService, CustomerService customerService)
        {
            _fiscalCodeService = fiscalCodeService;
            _cityService = csvCityService;
            _customerService = customerService;
        }

        public IActionResult Index()
        {
            return View();
        }

       
        public IActionResult FiscalCode()
        {
            return View();
        }
        public IActionResult GetProvinces()
        {
            var provinces = _cityService.GetProvinces().OrderBy(p => p.Name);
            return Json(provinces);
        }

        public IActionResult GetCities(string province)
        {
            var cities = _cityService.GetByProvince(province).OrderBy(c => c.Name);
            return Json(cities);
        }

        [HttpPost]
        public IActionResult FiscalCode(FiscalCodeViewModel model)
        {
            // validazione lato server
            if (ModelState.IsValid)
            {
                try
                {
                    var city = _cityService.GetCityById(model.BirthOfCity);
                    var fc = _fiscalCodeService.GenerateFiscalCode(new PersonalDataDto
                    {
                        BirthCity = city,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Birthday = model.Birthday,
                        Gender = model.Gender == 'F' ? Gender.Female : Gender.Male
                    });
                    TempData["FiscalCode"] = fc;
                    return RedirectToAction("Register", new { fiscalCode = fc });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("unattended_exception", "Si è verificato un problema nel calcolo");
                }
            }
            return View(model);
        }


        public IActionResult Register()
        {
            var model = new PersonalDataViewModel();

            // Recupera il codice fiscale da TempData
            if (TempData["FiscalCode"] != null)
            {
                model.CustomerTaxIdCode = TempData["FiscalCode"].ToString();
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Register(PersonalDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var customer = new Customer
                    {
                        CustomerName = model.FirstName,
                        CustomerSurname = model.LastName,
                        CustomerBirthCity = _cityService.GetCityById(model.BirthOfCity).Name,
                        CustomerAddress = model.Address,
                        CustomerCity = model.City,
                        CustomerZIPCode = model.ZIPCode,
                        CustomerEmail = model.Email,
                        CustomerHomeTelephone = model.HomeTelephone, // Assicurati di includere questo campo
                        CustomerTelephone = model.Telephone,
                        CustomerTaxIdCode = model.CustomerTaxIdCode, // Usa il codice fiscale dal modello
                        CustomerBirthday = model.Birthday,
                        Gender = model.Gender
                    };

                    _customerService.AddCustomer(customer); // Aggiungi il metodo nel servizio per salvare i dati
                    return RedirectToAction("Index"); // Redirect dopo la registrazione
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("registration_error", "Si è verificato un problema durante la registrazione");
                }
            }

            return View(model); // Ritorna alla vista con errori se la validazione fallisce
        }

    }
}
