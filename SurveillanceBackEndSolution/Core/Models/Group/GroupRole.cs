using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Models
{
    public class GroupRole
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
