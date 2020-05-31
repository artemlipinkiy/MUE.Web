using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MUE.Web.EntitiesDTO.BuildingDTO
{
    public class FlatDTO
    {
        [Display(Name = "ID")]
        public Guid FlatId { get; set; }
        [Display(Name = "Адрес")]
        public string Address { get; set; }
        [Display(Name = "Квартира")]
        public int Number { get; set; }
        [Display(Name = "Комнат")]
        public int Rooms { get; set; }
        [Display(Name = "Проживающих")]
        public int CountResidents { get; set; }
        [Display(Name = "Площадь")]
        public double Square { get; set; }
        [Display(Name = "ID дома")]
        public Guid BuildingId { get; set; }
        [Display(Name = "ID владельца")]
        public Guid? OwnersId { get; set; }
        [Display(Name = "Владелец")]
        public string OwnersFullName { get; set; }
    }
}