using CRMSystem.Api.Models;
using System.Linq.Expressions;

namespace CRMSystem.Api.Services;
public interface IUserService {
    Task<List<User>> GetListAsync(Expression<Func<User, bool>>? filter = null);
    Task<User?> GetAsync(Expression<Func<User, bool>>? filter = null);
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    void Delete(int id);
}