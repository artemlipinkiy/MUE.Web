using MUE.Web.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MUE.Web
{
    public class MUEContext :DbContext
    {
        public MUEContext() : base("DbConnection") { }
        public DbSet<Street> Streets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Building> Buildings { get; set; }

        public DbSet<Flat> Flats { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<MeterReading> MeterReadings { get; set; }
        public DbSet<Tariff> Tariffs { get; set; }
        public DbSet<ServiceBill> ServiceBills { get; set; }
        public DbSet<SettlementSheet> SettlementSheets { get; set; }
        public DbSet<TypeOfService> TypeOfServices { get; set; }
        public DbSet<Period> Periods { get; set; }
    }
}