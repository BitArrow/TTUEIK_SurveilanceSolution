using Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Interfaces
{
    public interface IGroupService
    {
        Task<ServiceResult> GetGroupListsAsync();
        Task<ServiceResult> GetGroupAsync(long id);
        Task<ServiceResult> AddGroup(GroupCreateDto dto);
        Task<ServiceResult> EditGroup(GroupEditDto dto);
        Task<ServiceResult> DeleteGroup(long groupId);
        Task<ServiceResult> GetSecurityDevice(long groupId, long deviceId);
        Task<ServiceResult> AddSecurityDevice(long groupId, GroupSecurityDeviceCreateDto dto);
        Task<ServiceResult> RemoveSecurityDevice(long groupId, long deviceId);
    }
}
