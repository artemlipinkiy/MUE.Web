using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MUE.Web.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Login { get; set; }
        public string HashPassword { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        public Guid? OwnerId { get; set; }
        public Owner Ownner { get; set; }
    }
}