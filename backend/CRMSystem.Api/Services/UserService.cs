using CRMSystem.Api.Models;
using CRMSystem.Api.Repositories;
using System.Linq.Expressions;

namespace CRMSystem.Api.Services;
public class UserService(IUserRepository userRepository) : IUserService {
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<List<User>> GetListAsync(Expression<Func<User, bool>>? filter = null) {
        return await _userRepository.GetUsersAsync(filter);
    }

    public async Task<User?> GetAsync(Expression<Func<User, bool>>? filter = null) {
        return await _userRepository.GetUserAsync(filter);
    }

    public async Task<User> CreateAsync(User user) {
        return await _userRepository.CreateUserAsync(user);
    }

    public async Task<User> UpdateAsync(User user) {
        return await _userRepository.UpdateUserAsync(user);
    }

    public void Delete(int id) {
        _userRepository.DeleteUserAsync(id);
    }
}