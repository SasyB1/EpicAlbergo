using System;
using Microsoft.Data.SqlClient;
using EpicAlbergo.Models.Dto;
using EpicAlbergo.Models;
using EpicAlbergo.Interfaces;

namespace EpicAlbergo.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IFiscalCodeService _fiscalCodeService;
        private readonly ICsvCityService _cityService;
        private readonly IConfiguration _config;

        public CustomerService(IFiscalCodeService fiscalCodeService, ICsvCityService cityService, IConfiguration config)
        {
            _fiscalCodeService = fiscalCodeService;
            _cityService = cityService;
            _config = config;
        }

        public void AddCustomer(CustomerDto customer)
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
                        cmd.Parameters.AddWithValue("@CustomerHomeTelephone", customer.CustomerHomeTelephone);
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

        public List<CustomerDto> GetCustomers()
        {
            var customers = new List<CustomerDto>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    conn.Open();
                    const string SELECT_COMMAND = "SELECT CustomerId, CustomerName, CustomerSurname FROM Customers";
                    using (SqlCommand cmd = new SqlCommand(SELECT_COMMAND, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var customer = new CustomerDto
                                {
                                    CustomerId = reader.GetInt32(0),
                                    CustomerName = reader.GetString(1),
                                    CustomerSurname = reader.GetString(2)
                                };
                                customers.Add(customer);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Logga o gestisci l'errore
                throw new Exception("Errore nel recupero dei clienti: " + ex.Message, ex);
            }
            return customers;
        }

        public List<Customer> GetCustomersByPartialFiscalCode(string partialFiscalCode)
        {
            var customers = new List<Customer>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    conn.Open();
                    const string query = "SELECT * FROM Customers WHERE CustomerTaxIdCode LIKE @PartialFiscalCode";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PartialFiscalCode", "%" + partialFiscalCode + "%");
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var customer = new Customer
                                {
                                    CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                    CustomerSurname = reader.GetString(reader.GetOrdinal("CustomerSurname")),
                                    CustomerName = reader.GetString(reader.GetOrdinal("CustomerName")),
                                    CustomerBirthCity = reader.GetString(reader.GetOrdinal("CustomerBirthCity")),
                                    CustomerAddress = reader.GetString(reader.GetOrdinal("CustomerAddress")),
                                    CustomerCity = reader.GetString(reader.GetOrdinal("CustomerCity")),
                                    CustomerZIPCode = reader.GetString(reader.GetOrdinal("CustomerZIPCode")),
                                    CustomerEmail = reader.GetString(reader.GetOrdinal("CustomerEmail")),
                                    CustomerHomeTelephone = reader.GetString(reader.GetOrdinal("CustomerHomeTelephone")),
                                    CustomerTelephone = reader.GetString(reader.GetOrdinal("CustomerTelephone")),
                                    CustomerTaxIdCode = reader.GetString(reader.GetOrdinal("CustomerTaxIdCode")),
                                    CustomerBirthday = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("CustomerBirthday"))),
                                    Gender = reader.GetString(reader.GetOrdinal("Gender")).FirstOrDefault()
                                };
                                customers.Add(customer);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore nel recupero dei clienti: " + ex.Message, ex);
            }
            return customers;
        }



    }
}