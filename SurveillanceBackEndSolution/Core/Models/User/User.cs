using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Models
{
    public class User
    {
        [Key]
        public long Id { get; set; }

        public string B2CId { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(32)]
        public string Phone { get; set; }

        [MaxLength(128)]
        public string Email { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public virtual List<UserDevice> Devices { get; set; } = new List<UserDevice>();

        public virtual List<GroupUser> Groups { get; set; } = new List<GroupUser>();
    }
}
