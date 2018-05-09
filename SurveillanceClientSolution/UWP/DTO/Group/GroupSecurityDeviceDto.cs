using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP.DTO
{
    public class GroupSecurityDeviceDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string DeviceId { get; set; }

        public string DeviceKey { get; set; }

        public long GroupId { get; set; }
    }
}
