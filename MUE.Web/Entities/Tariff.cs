using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MUE.Web.Entities
{
    public class Tariff
    {
        public Guid TariffId { get; set; }
        public TypeOfService TypeOfService { get; set; }
        public Guid TypeOfServiceId { get; set; }
        public double Value { get; set; }
        public Guid FlatId { get; set; }
        public Flat Flat { get; set; }
    }
}