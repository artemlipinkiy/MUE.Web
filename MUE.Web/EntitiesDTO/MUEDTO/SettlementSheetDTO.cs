using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MUE.Web.EntitiesDTO.MUEDTO
{
    public class SettlementSheetDTO
    {
        [Display(Name = "ID")]
        public Guid SettlementSheetId { get; set; }
        [Display(Name = "Период")]
        public string Period { get; set; }
        [Display(Name = "ID период")]
        public Guid PeriodId { get; set; }
        [Display(Name = "Квартира")]
        public string Flat { get; set; }
        [Display(Name = "ID квартиры")]
        public Guid FlatId { get; set; }
        [Display(Name = "Итог")]
        public double AmmountToBePaid { get; set; }
        [Display(Name = "Статус")]
        public int Status { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }

    }
}