using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MUE.Web.Entities
{
    public class Flat
    {
        public Guid FlatId { get; set; }
        public int Number { get; set; }
        public int Rooms { get; set; }
        public int CountResidents { get; set; }
        public double Square { get; set; }
        public Building Building { get; set; }
        public Guid BuildingId { get; set; }
        public Owner Owner { get; set; }
        public Guid? OwnersId { get; set; }
    }
}