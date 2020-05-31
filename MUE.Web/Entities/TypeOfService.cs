using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MUE.Web.Entities
{
    public class TypeOfService
    {
        public Guid TypeOfServiceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UnitOfMeasurment { get; set; }
        public bool IsMeter { get; set; }
    }
}