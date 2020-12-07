using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Context.Entities
{
    [Table("Roles", Schema = "User")]
    public class Role : IdentityRole<int>
    {
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int? AddedById { get; set; }
        public User AddedBy { get; set; }
        public int? UpdatedById { get; set; }
        public User UpdatedBy { get; set; }
    }
}
