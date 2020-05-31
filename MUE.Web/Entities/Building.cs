using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MUE.Web.Entities
{
    public class Building
    {
        public Guid BuildingId { get; set; }
        public string Number { get; set; }
        public string Description { get; set; }
        public Street Street { get; set; }
        public Guid StreetId { get; set; }
    }
}