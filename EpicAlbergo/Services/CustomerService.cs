using System;
using Microsoft.Data.SqlClient;
using EpicAlbergo.Models.Dto;
using EpicAlbergo.Models;

namespace EpicAlbergo.Services
{
    public class CustomerService
    {
        private readonly FiscalCodeService _fiscalCodeService;
        private readonly CsvCityService _cityService;
        private readonly IConfiguration _config;

        public CustomerService(FiscalCodeService fiscalCodeService, CsvCityService cityService, IConfiguration config)
        {
            _fiscalCodeService = fiscalCodeService;
            _cityService = cityService;
            _config = config;
        }

        public void AddCustomer(Customer customer)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    conn.Open();
                    const string INSERT_CUSTOMER = @"
                INSERT INTO Customers (
                    CustomerSurname,
                    CustomerName,
                    CustomerBirthCity,
                    CustomerAddress,
                    CustomerCity,
                    CustomerZIPCode,
                    CustomerEmail,
                    CustomerHomeTelephone,
                    CustomerTelephone,
                    CustomerTaxIdCode,
                    CustomerBirthday,
                    Gender
                ) VALUES (
                    @CustomerSurname,
                    @CustomerName,
                    @CustomerBirthCity,
                    @CustomerAddress,
                    @CustomerCity,
                    @CustomerZIPCode,
                    @CustomerEmail,
                    @CustomerHomeTelephone,
                    @CustomerTelephone,
                    @CustomerTaxIdCode,
                    @CustomerBirthday,
                    @Gender
                )";

                    using (SqlCommand cmd = new SqlCommand(INSERT_CUSTOMER, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerSurname", customer.CustomerSurname);
                        cmd.Parameters.AddWithValue("@CustomerName", customer.CustomerName);
                        cmd.Parameters.AddWithValue("@CustomerBirthCity", customer.CustomerBirthCity);
                        cmd.Parameters.AddWithValue("@CustomerAddress", customer.CustomerAddress);
                        cmd.Parameters.AddWithValue("@CustomerCity", customer.CustomerCity);
                        cmd.Parameters.AddWithValue("@CustomerZIPCode", customer.CustomerZIPCode);
                        cmd.Parameters.AddWithValue("@CustomerEmail", customer.CustomerEmail);
                        cmd.Parameters.AddWithValue("@CustomerHomeTelephone",customer.CustomerHomeTelephone);
                        cmd.Parameters.AddWithValue("@CustomerTelephone", customer.CustomerTelephone);
                        cmd.Parameters.AddWithValue("@CustomerTaxIdCode", customer.CustomerTaxIdCode);
                        cmd.Parameters.AddWithValue("@CustomerBirthday", customer.CustomerBirthday);
                        cmd.Parameters.AddWithValue("@Gender", customer.Gender);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'inserimento del cliente", ex);
            }
        }

    }
}
