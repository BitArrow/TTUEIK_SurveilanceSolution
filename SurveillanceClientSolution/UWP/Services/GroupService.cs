using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP.DTO;
using UWP.Services.Interfaces;

namespace UWP.Services
{
    public class GroupService : APIBaseService, IGroupService
    {
        public GroupService() 
        {

        }

        public async Task<List<GroupDto>> GetListAsync()
        {
            var result = await GetAsync<List<GroupDto>>("group");

            return result;
        }

        public async Task<GroupDto> GetGroupAsync(long id)
        {
            var result = await GetAsync<GroupDto>($"group/{id}");
            return result;
        }

        public async Task AddGroupAsync(string name)
        {
            var groupDto = new GroupCreateDto
            {
                Name = name
            };
            var result = await PostAsync<GroupDto>("group", groupDto);
        }

        public async Task<GroupDto> UpdateGroupAsync(GroupDto group)
        {
            var result = await PutAsync<GroupDto>("group", group);
            return result;
        }

        public async Task DeleteGroupAsync(long id)
        {
            await DeleteAsync($"group/{id}");
        }

        public async Task<GroupSecurityDeviceDto> GetSecurityDevice(long groupId, long deviceId)
        {
            var result = await GetAsync<GroupSecurityDeviceDto>($"group/{groupId}/device/{deviceId}");
            return result;
        }

        public async Task<GroupSecurityDeviceDto> AddNewSecurityDevice(string name, long groupId)
        {
            var createDevice = new GroupSecurityDeviceCreateDto
            {
                Name = name
            };
            var result = await PostAsync<GroupSecurityDeviceDto>($"group/{groupId}/device", createDevice);
            return result;
        }

        public async Task DeleteSecurityDevice(long groupId, long deviceId)
        {
            await DeleteAsync($"group/{groupId}/device/{deviceId}");
        }
    }
}
