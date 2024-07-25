using EpicAlbergo.Models.Dto;
using EpicAlbergo.Models;

namespace EpicAlbergo.Interfaces
{
    public interface ICustomerService
    {
        void AddCustomer(CustomerDto customer);
        List<CustomerDto> GetCustomers();
        List<Customer> GetCustomersByPartialFiscalCode(string partialFiscalCode);
    }
}
