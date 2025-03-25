using CRMSystem.Api.Models;
using System.Linq.Expressions;

namespace CRMSystem.Api.Repositories;
public interface ICustomersRepository {
    Task<List<Customer>> GetCustomersAsync(Expression<Func<Customer, bool>>? filter = null);
    Task<Customer?> GetCustomerAsync(Expression<Func<Customer, bool>>? filter = null);
    Task<Customer> CreateCustomerAsync(Customer customer);
    Task<Customer> UpdateCustomerAsync(Customer customer);
    void DeleteCustomerAsync(int id);
}