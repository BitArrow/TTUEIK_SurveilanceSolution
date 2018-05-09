using AutoMapper;
using Core.DTO;
using Core.DTO.Constants;
using Core.Models;
using Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class GroupService : IGroupService
    {
        private readonly SurveillanceContext _db;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IIoTService _ioTService;

        public GroupService(SurveillanceContext db, IUserService userService, IMapper mapper,
            IIoTService ioTService)
        {
            _db = db;
            _userService = userService;
            _mapper = mapper;
            _ioTService = ioTService;
        }

        public async Task<ServiceResult> GetGroupListsAsync()
        {
            var user = await _userService.GetUser();

            if (user == null)
                return ServiceResult.Error(ErrorsEnum.UserB2CMissing);

            var groups = await _db.Groups
                .Include(g => g.GroupUsers)
                .Where(g => g.OwnerId == user.Id || g.GroupUsers.Any(x => x.UserId == user.Id))
                .ToListAsync();

            return ServiceResult.Ok(_mapper.Map<List<GroupListDto>>(groups)); 
        }

        public async Task<ServiceResult> GetGroupAsync(long id)
        {
            var user = await _userService.GetUser();

            if (user == null)
                return ServiceResult.Error(ErrorsEnum.UserB2CMissing);

            var group = await _db.Groups
                .Include(g => g.GroupUsers)
                .ThenInclude(gu => gu.GroupRole)
                .Include(g => g.GroupUsers)
                .ThenInclude(gu => gu.User)
                .Include(g => g.GroupSecurityDevices)
                .Where(g => g.Id == id &&
                        (g.OwnerId == user.Id || g.GroupUsers.Any(gu => gu.UserId == user.Id)))
                .FirstOrDefaultAsync();

            if (group == null)
                return ServiceResult.Error(ErrorsEnum.GroupNotFound);

            return ServiceResult.Ok(_mapper.Map<GroupDto>(group));
        }

        public async Task<ServiceResult> AddGroup(GroupCreateDto dto)
        {
            var user = await _userService.GetUser();

            if (user == null)
                return ServiceResult.Error(ErrorsEnum.UserB2CMissing);

            var newGroup = new Group
            {
                Name = dto.Name,
                OwnerId = user.Id,
            };

            var group = await _db.Groups.AddAsync(newGroup);
            await _db.SaveChangesAsync();

            return ServiceResult.Ok(_mapper.Map<GroupDto>(group.Entity));
        }

        public async Task<ServiceResult> EditGroup(GroupEditDto dto)
        {
            var user = await _userService.GetUser();

            if (user == null)
                return ServiceResult.Error(ErrorsEnum.UserB2CMissing);

            var existing = await _db.Groups
                .Where(g => g.Id == dto.Id && g.OwnerId == user.Id)
                .FirstOrDefaultAsync();

            if (existing == null)
                return ServiceResult.Error(ErrorsEnum.GroupNotFound);

            existing.Name = dto.Name;

            await _db.SaveChangesAsync();

            return ServiceResult.Ok(_mapper.Map<GroupDto>(existing));
        }

        public async Task<ServiceResult> DeleteGroup(long groupId)
        {
            var user = await _userService.GetUser();

            if (user == null)
                return ServiceResult.Error(ErrorsEnum.UserB2CMissing);

            var existing = await _db.Groups
                .Where(g => g.Id == groupId && g.OwnerId == user.Id)
                .FirstOrDefaultAsync();

            if (existing == null)
                return ServiceResult.Error(ErrorsEnum.GroupNotFound);

            _db.Groups.Remove(existing);
            await _db.SaveChangesAsync();

            return ServiceResult.Ok();
        }

        public async Task<ServiceResult> GetSecurityDevice(long groupId, long deviceId)
        {
            var user = await _userService.GetUser();

            if (user == null)
                return ServiceResult.Error(ErrorsEnum.UserB2CMissing);

            var device = await _db.GroupSecurityDevices
                .Include(d => d.Group)
                .Where(d => d.Id == deviceId && d.GroupId == groupId && d.Group.OwnerId == user.Id)
                .FirstOrDefaultAsync();

            if (device == null)
                return ServiceResult.Error(ErrorsEnum.GroupSecurityDeviceNotFound);

            return ServiceResult.Ok(_mapper.Map<GroupSecurityDeviceDto>(device));
        }

        public async Task<ServiceResult> AddSecurityDevice(long groupId, GroupSecurityDeviceCreateDto dto)
        {
            var user = await _userService.GetUser();

            if (user == null)
                return ServiceResult.Error(ErrorsEnum.UserB2CMissing);

            var group = await _db.Groups
                .Include(g => g.GroupSecurityDevices)
                .Where(g => g.Id == groupId && g.OwnerId == user.Id)
                .FirstOrDefaultAsync();

            if (group == null)
                return ServiceResult.Error(ErrorsEnum.GroupNotFound);

            var newDevice = await _ioTService.RegisterNewDevice();

            group.GroupSecurityDevices.Add(new GroupSecurityDevice
            {
                DeviceId = newDevice.DeviceId,
                DeviceKey = newDevice.DeviceKey,
                GroupId = groupId,
                Name = dto.Name
            });

            await _db.SaveChangesAsync();

            return ServiceResult.Ok(_mapper.Map<GroupSecurityDeviceDto>(group.GroupSecurityDevices.Last()));
        }

        public async Task<ServiceResult> RemoveSecurityDevice(long groupId, long deviceId)
        {
            var user = await _userService.GetUser();

            if (user == null)
                return ServiceResult.Error(ErrorsEnum.UserB2CMissing);

            var device = await _db.GroupSecurityDevices
                .Include(d => d.Group)
                .Where(d => d.Id == deviceId && d.GroupId == groupId && d.Group.OwnerId == user.Id)
                .FirstOrDefaultAsync();

            if (device == null)
                return ServiceResult.Error(ErrorsEnum.GroupSecurityDeviceNotFound);

            await _ioTService.DeleteDevice(device.DeviceId);
            _db.GroupSecurityDevices.Remove(device);

            await _db.SaveChangesAsync();

            return ServiceResult.Ok();
        }

        //public async Task<ServiceResult> AddGroupUser(GroupUserDto dto)
        //{
        //    var user = await _userService.GetUser();

        //    if (user == null)
        //        return ServiceResult.Error(ErrorsEnum.UserB2CMissing);

        //    var newGroup = new GroupUser
        //    {
        //        Name = dto.Name,
        //        OwnerId = user.Id,
        //    };

        //    var group = await _db.Groups.AddAsync(newGroup);
        //    await _db.SaveChangesAsync();

        //    return ServiceResult.Ok(_mapper.Map<GroupDto>(group.Entity));
        //}

        //public async Task<ServiceResult> EditGroupUser(GroupCreateDto dto)
        //{
        //    var user = await _userService.GetUser();

        //    if (user == null)
        //        return ServiceResult.Error(ErrorsEnum.UserB2CMissing);

        //    var newGroup = new Group
        //    {
        //        Name = dto.Name,
        //        OwnerId = user.Id,
        //    };

        //    var group = await _db.Groups.AddAsync(newGroup);
        //    await _db.SaveChangesAsync();

        //    return ServiceResult.Ok(_mapper.Map<GroupDto>(group.Entity));
        //}

        //public async Task<ServiceResult> RemoveGroupUser(GroupCreateDto dto)
        //{
        //    var user = await _userService.GetUser();

        //    if (user == null)
        //        return ServiceResult.Error(ErrorsEnum.UserB2CMissing);

        //    var newGroup = new Group
        //    {
        //        Name = dto.Name,
        //        OwnerId = user.Id,
        //    };

        //    var group = await _db.Groups.AddAsync(newGroup);
        //    await _db.SaveChangesAsync();

        //    return ServiceResult.Ok(_mapper.Map<GroupDto>(group.Entity));
        //}
    }
}
