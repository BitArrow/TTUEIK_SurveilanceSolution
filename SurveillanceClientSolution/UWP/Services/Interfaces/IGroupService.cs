using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP.DTO;

namespace UWP.Services.Interfaces
{
    public interface IGroupService
    {
        Task<List<GroupDto>> GetListAsync();
        Task<GroupDto> GetGroupAsync(long id);
        Task AddGroupAsync(string name);
        Task<GroupDto> UpdateGroupAsync(GroupDto group);
        Task DeleteGroupAsync(long id);
        Task<GroupSecurityDeviceDto> GetSecurityDevice(long groupId, long deviceId);
        Task<GroupSecurityDeviceDto> AddNewSecurityDevice(string name, long groupId);
        Task DeleteSecurityDevice(long groupId, long deviceId);
    }
}
