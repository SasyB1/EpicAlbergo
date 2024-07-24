using Microsoft.AspNetCore.Mvc;
using EpicAlbergo.Services;
using EpicAlbergo.Models.Dto;
using EpicAlbergo.Models;
using Newtonsoft.Json;


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
        public IActionResult FiscalCode(FiscalCodeDto model)
        {
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
                    var customerData = new
                    {
                        FiscalCode = fc,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Birthday = model.Birthday.ToString("yyyy-MM-dd"),
                        Gender = model.Gender,
                    };

                    TempData["CustomerData"] = JsonConvert.SerializeObject(customerData);
                    return RedirectToAction("Register");
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
            var model = new CustomerDto();
            if (TempData["CustomerData"] != null)
            {
                var customerDataJson = TempData["CustomerData"].ToString();
                var customerData = JsonConvert.DeserializeObject<dynamic>(customerDataJson);

                model.CustomerTaxIdCode = customerData.FiscalCode;
                model.CustomerName = customerData.FirstName;
                model.CustomerSurname = customerData.LastName;
                model.CustomerBirthday = DateOnly.Parse(customerData.Birthday.ToString()); 
                model.Gender = customerData.Gender.ToString()[0]; 
            }

            return View(model);
        }


        [HttpPost]
        public IActionResult Register(CustomerDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _customerService.AddCustomer(model); 
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("registration_error", "Si è verificato un problema durante la registrazione");
                }
            }
            return View(model); 
        }

    }
}
