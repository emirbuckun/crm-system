using CRMSystem.Api.Data;
using CRMSystem.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CRMSystem.Api.Repositories;

public interface ICustomersRepository {
    Task<List<Customer>> GetCustomersAsync(Expression<Func<Customer, bool>>? filter = null);
    Task<Customer?> GetCustomerAsync(Expression<Func<Customer, bool>>? filter = null);
    Task<Customer> CreateCustomerAsync(Customer customer);
    Task<Customer> UpdateCustomerAsync(Customer customer);
    void DeleteCustomerAsync(int id);
}

public class CustomerRepository(AppDbContext context) : ICustomersRepository {
    protected readonly AppDbContext _context = context;

    public async Task<Customer> CreateCustomerAsync(Customer customer) {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async void DeleteCustomerAsync(int id) {
        var customer = _context.Customers.Find(id);
        if (customer != null) {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Customer?> GetCustomerAsync(Expression<Func<Customer, bool>>? filter = null) {
        return await _context.Customers.SingleOrDefaultAsync(filter ?? (_ => true));
    }

    public async Task<List<Customer>> GetCustomersAsync(Expression<Func<Customer, bool>>? filter = null) {
        return await _context.Customers.Where(filter ?? (_ => true)).ToListAsync();
    }

    public async Task<Customer> UpdateCustomerAsync(Customer customer) {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
        return customer;
    }
}