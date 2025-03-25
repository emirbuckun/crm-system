using CRMSystem.Api.Models;
using System.Linq.Expressions;

namespace CRMSystem.Api.Repositories;
public interface IUserRepository {
    Task<List<User>> GetUsersAsync(Expression<Func<User, bool>>? filter = null);
    Task<User?> GetUserAsync(Expression<Func<User, bool>>? filter = null);
    Task<User> CreateUserAsync(User user);
    Task<User> UpdateUserAsync(User user);
    void DeleteUserAsync(int id);
}