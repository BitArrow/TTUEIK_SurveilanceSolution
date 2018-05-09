using AutoMapper;
using Core.DTO;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User
            CreateMap<User, UserDto>();

            // Group
            CreateMap<Group, GroupDto>();
            CreateMap<Group, GroupListDto>();
            CreateMap<GroupSecurityDevice, GroupSecurityDeviceDto>();
            CreateMap<GroupUser, GroupUserDto>();
        }
    }
}
