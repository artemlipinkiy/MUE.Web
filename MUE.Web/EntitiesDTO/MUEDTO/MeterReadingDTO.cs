using MUE.Web.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MUE.Web.EntitiesDTO.MUEDTO
{
    public class MeterReadingDTO
    {
        [Display(Name = "ID")]
        public Guid MeterReadingId { get; set; }
        [Display(Name = "Показания")]
        public double Value { get; set; }
        [Display(Name = "Период")]
        public string CodePeriod { get; set; }
        [Display(Name = "ID периода")]
        public Guid PeriodId { get; set; }
        [Display(Name = "Услуга")]
        public string TypeOfService { get; set; }

        [Display(Name = "ID услуги")]
        public Guid TypeofServiceId { get; set; }

        [Display(Name = "Квартира")]
        public string Flat { get; set; }
        [Display(Name = "ID квартиры")]
        public Guid FlatId { get; set; }
    }
}