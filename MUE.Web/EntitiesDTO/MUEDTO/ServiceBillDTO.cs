using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MUE.Web.EntitiesDTO.MUEDTO
{
    public class ServiceBillDTO
    {

        [Display(Name = "ID")]
        public Guid ServiceBillId { get; set; }

        [Display(Name = "Период")]
        public string Period { get; set; }
        [Display(Name = "ID периода")]
        public Guid PeriodId { get; set; }
        [Display(Name = "Услуга")]
        public string TypeOfService { get; set; }

        [Display(Name = "ID услуги")]
        public Guid TypeOfServiceId { get; set; }

        [Display(Name = "Квартира")]
        public string Flat { get; set; }
        [Display(Name = "ID квартиры")]
        public Guid FlatId { get; set; }

        [Display(Name = "Статус")]
        public int Status { get; set; }

        [Display(Name = "Сумма к оплате")]
        public double Summ { get; set; }
    }
}