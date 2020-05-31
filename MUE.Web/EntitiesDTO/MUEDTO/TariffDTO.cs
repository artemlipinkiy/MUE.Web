using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MUE.Web.EntitiesDTO.MUEDTO
{
    public class TariffDTO
    {
        [Display(Name = "ID")]
        public Guid TariffId { get; set; }
        [Display(Name = "Услуга")]
        public string TypeOfService { get; set; }
        [Display(Name = "ID услуги")]
        public Guid TypeOfServiceId { get; set; }
        [Display(Name = "ID квартиры")]
        public Guid FlatId { get; set; }
        [Display(Name = "Квартира")]
        public string Flat { get; set; }
        [Display(Name = "Цена за ед.")]
        public double Value { get; set; }
        [Display(Name = "Ед. измерения")]
        public string UnitOfMeasurment { get; set; }

        [Display(Name = "Счетчик")]
        public bool IsMeter { get; set; }
        //public List<Flat> Flats { get; set; }
    }
}