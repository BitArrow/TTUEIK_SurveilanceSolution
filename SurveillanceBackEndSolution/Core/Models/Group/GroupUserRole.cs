using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Models
{
    public class GroupUserRole
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("Role")]
        public long RoleId { get; set; }
        public virtual GroupRole Role { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }
        public virtual User User { get; set; }

    }
}
