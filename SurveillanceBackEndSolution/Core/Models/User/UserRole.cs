using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Models
{
    public class UserRole
    {
        // Global roles
        // Eg. Administrator, User
        [Key]
        public long Id { get; set; }

        [DisplayName("Role name")]
        public string Name { get; set; }

        public virtual List<User> Users { get; set; } = new List<User>();
    }
}
