using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Models
{
    public class Device
    {
        [Key]
        public long Id { get; set; }

        public string ReferenceId { get; set; }

        public bool CheckLocation { get; set; }

        public bool Active { get; set; }

        [MaxLength(25)]
        public string IP { get; set; }

        public virtual List<UserDevice> UserDevices { get; set; } = new List<UserDevice>();
    }
}
