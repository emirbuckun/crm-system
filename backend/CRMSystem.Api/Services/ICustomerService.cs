using CRMSystem.Api.Models;
using System.Linq.Expressions;

namespace CRMSystem.Api.Services;
public interface ICustomerService {
    Task<List<Customer>> GetListAsync(Expression<Func<Customer, bool>>? filter = null);
    Task<Customer?> GetAsync(Expression<Func<Customer, bool>>? filter = null);
    Task<Customer> CreateAsync(Customer customer);
    Task<Customer> UpdateAsync(Customer customer);
    void Delete(int id);
}