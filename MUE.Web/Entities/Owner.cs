using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MUE.Web.Entities
{
    public class Owner
    {
        public Guid OwnerId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddlleName { get; set; }
        public string Status { get; set; }
    }
}