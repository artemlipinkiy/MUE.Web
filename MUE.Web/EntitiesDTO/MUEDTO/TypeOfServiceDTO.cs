using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MUE.Web.EntitiesDTO.MUEDTO
{
    public class TypeOfServiceDTO
    {
        [Display(Name = "ID")]
        public Guid TypeOfServiceId { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Ед. измерения")]
        public string UnitOfMeasurment { get; set; }
        [Display(Name = "Счетчик")]
        public bool IsMeter { get; set; }
    }
}