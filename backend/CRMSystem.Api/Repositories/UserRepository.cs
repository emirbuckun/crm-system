using CRMSystem.Api.Data;
using CRMSystem.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CRMSystem.Api.Repositories;
public class UserRepository(AppDbContext context) : IUserRepository {
    private readonly AppDbContext _context = context;

    public async Task<User> CreateUserAsync(User user) {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async void DeleteUserAsync(int id) {
        var user = _context.Users.Find(id);
        if (user != null) {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<User?> GetUserAsync(Expression<Func<User, bool>>? filter = null) {
        return await _context.Users.SingleOrDefaultAsync(filter ?? (_ => true));
    }

    public async Task<List<User>> GetUsersAsync(Expression<Func<User, bool>>? filter = null) {
        return await _context.Users.Where(filter ?? (_ => true)).ToListAsync();
    }

    public async Task<User> UpdateUserAsync(User user) {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }
}