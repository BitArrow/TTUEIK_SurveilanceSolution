using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Models
{
    public class GroupSecurityDevice
    {
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Friendly name
        /// </summary>
        [MinLength(1)]
        [MaxLength(128)]
        public string Name { get; set; }

        public string DeviceId { get; set; }

        public string DeviceKey { get; set; }

        [ForeignKey("Group")]
        public long GroupId { get; set; }

        public Group Group { get; set; }
    }
}
