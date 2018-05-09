using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTO
{
    public class GroupDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public List<GroupUserDto> GroupUsers { get; set; }

        public List<GroupSecurityDeviceDto> GroupSecurityDevices { get; set; }
    }
}
