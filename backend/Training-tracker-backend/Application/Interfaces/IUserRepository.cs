using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<User>> GetAllUsersAsync();
        public Task<User> GetUserByIdAsync(int id);
        public Task<int> AddUserAsync(User user);
        public Task UpdateUserAsync(User user);

        public Task DeleteUserAsync(User user);
        Task<bool> ExistsAsync(int entityId);
    }

}
