using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Models
{
    public class Group
    {
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Friendly name
        /// </summary>
        [MinLength(1)]
        [MaxLength(128)]
        public string Name { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        [ForeignKey("Owner")]
        public long OwnerId { get; set; }

        public virtual User Owner { get; set; }

        /// <summary>
        /// Key for internal use. 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Guid SystemKey { get; set; }

        public virtual List<GroupUser> GroupUsers { get; set; } = new List<GroupUser>();

        public virtual List<GroupSecurityDevice> GroupSecurityDevices { get; set; } = new List<GroupSecurityDevice>();
    }
}
