﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTO
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