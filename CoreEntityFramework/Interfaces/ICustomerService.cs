using CoreEntityFramework.Models;

namespace CoreEntityFramework.Interfaces
{
    public interface ICustomerService
    {
        // definitions for implementations of actions to create, read, update, delete

        // base CRUD
        Task<IEnumerable<CustomerDto>> ListCustomers();

        Task<CustomerDto?> FindCustomer(int id);

        Task<ServiceResponse> UpdateCustomer(CustomerDto CustomerDto);

        Task<ServiceResponse> AddCustomer(CustomerDto CustomerDto);

        Task<ServiceResponse> DeleteCustomer(int id);

        // related methods

    }
}
