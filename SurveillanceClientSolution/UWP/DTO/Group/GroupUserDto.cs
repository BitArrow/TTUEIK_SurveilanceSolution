using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP.DTO
{
    public class GroupUserDto
    {
        public long Id { get; set; }

        public bool Track { get; set; }

        public bool Notify { get; set; }

        public long RoleId { get; set; }

        public string GroupRole { get; set; }

        public long GroupId { get; set; }

        public long UserId { get; set; }

        public UserDto User { get; set; }
    }
}
