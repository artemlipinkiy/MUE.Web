using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MUE.Web.EntitiesDTO.MUEDTO
{
    public class CreateTariffDTO
    {

        [Display(Name = "Название услуги")]
        public string NameService { get; set; }
        [Display(Name = "Цена за ед.")]
        public double Value { get; set; }
    }
}