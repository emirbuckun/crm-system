using CRMSystem.Api.Models;
using CRMSystem.Api.Repositories;
using System.Linq.Expressions;

namespace CRMSystem.Api.Services;

public interface ICustomerService {
    Task<List<Customer>> GetListAsync(Expression<Func<Customer, bool>>? filter = null);
    Task<Customer?> GetAsync(Expression<Func<Customer, bool>>? filter = null);
    Task<Customer> CreateAsync(Customer customer);
    Task<Customer> UpdateAsync(Customer customer);
    Task<bool> Delete(int id);
}

public class CustomerService(ICustomersRepository customersRepository) : ICustomerService {
    private readonly ICustomersRepository _customersRepository = customersRepository;

    public async Task<List<Customer>> GetListAsync(Expression<Func<Customer, bool>>? filter = null) {
        return await _customersRepository.GetCustomersAsync(filter);
    }

    public async Task<Customer?> GetAsync(Expression<Func<Customer, bool>>? filter = null) {
        return await _customersRepository.GetCustomerAsync(filter);
    }

    public async Task<Customer> CreateAsync(Customer customer) {
        return await _customersRepository.CreateCustomerAsync(customer);
    }

    public async Task<Customer> UpdateAsync(Customer customer) {
        return await _customersRepository.UpdateCustomerAsync(customer);
    }

    public async Task<bool> Delete(int id) {
        return await _customersRepository.DeleteCustomerAsync(id);
    }
}