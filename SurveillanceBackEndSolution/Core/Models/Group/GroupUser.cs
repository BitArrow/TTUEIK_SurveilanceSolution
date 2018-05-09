using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Models
{
    public class GroupUser
    {
        // Group specific roles
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Should this user's device be tracked
        /// </summary>
        public bool Track { get; set; }

        /// <summary>
        /// Should this user be notified
        /// </summary>
        public bool Notify { get; set; }

        [ForeignKey("GroupRole")]
        public long RoleId { get; set; }

        public virtual GroupRole GroupRole { get; set; }

        [ForeignKey("Group")]
        public long GroupId { get; set; }

        public virtual Group Group { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }

        public virtual User User { get; set; }
    }
}
