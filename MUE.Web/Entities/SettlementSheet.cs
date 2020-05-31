using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MUE.Web.Entities
{
    public class SettlementSheet
    {
        public Guid SettlementSheetId { get; set; }
        public Period Period { get; set; }
        public Guid PeriodId { get; set; }
        public Flat Flat { get; set; }
        public Guid FlatId { get; set; }
        public double AmmountToBePaid { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
    }
}