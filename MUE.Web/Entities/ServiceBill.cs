using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MUE.Web.Entities
{
    public class ServiceBill
    {
        public Guid ServiceBillId { get; set; }
        public Period Period { get; set; }
        public Guid PeriodId { get; set; }
        public TypeOfService TypeOfService { get; set; }
        public Guid TypeOfServiceId { get; set; }
        public Flat Flat { get; set; }
        public Guid FlatId { get; set; }
        public int Status { get; set; }
        public double Summ { get; set; }
    }
}