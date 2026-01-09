using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using GameCollection.Domain.Common;

namespace GameCollection.Domain.Entities
{
    public class User : BaseEntity
    {
        // Mandatory Values
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        // User Role
        public string Role { get; set; } = "User";  // "Admin", "User"

        // Navigation Properties
        public virtual ICollection<GameCollection> Collections { get; set; } = new HashSet<GameCollection>();
        public virtual ICollection<UserReview> Reviews { get; set; } = new HashSet<UserReview>();
    }
}
