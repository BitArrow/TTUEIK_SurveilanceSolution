using Core.DTO;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Interfaces
{
    public interface IUserService
    {
        string UserB2CId { get ; }
        string UserDisplayName { get; }

        Task<ServiceResult> ValidateUser();

        Task<User> GetUser();

        Task<User> GetUser(long id);

        Task<List<Group>> GetUserGroups(long userId);
    }
}
