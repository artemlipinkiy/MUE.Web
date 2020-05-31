using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MUE.Web.Entities
{
    public class Street
    {
        public Guid StreetId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}