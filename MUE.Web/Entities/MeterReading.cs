using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MUE.Web.Entities
{
    public class MeterReading
    {
        public Guid MeterReadingId { get; set; }
        public double Value { get; set; }
        public Period Period { get; set; }
        public Guid PeriodId { get; set; }
        public TypeOfService TypeOfService { get; set; }
        public Guid TypeofServiceId { get; set; }
        public Flat Flat { get; set; }
        public Guid FlatId { get; set; }
    }
}