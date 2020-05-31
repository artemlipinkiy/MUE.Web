using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MUE.Web.Entities
{
    public class Role
    {
        public Guid RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}