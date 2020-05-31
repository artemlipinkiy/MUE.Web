using MUE.Web.EntitiesDTO.MUEDTO;
using MUE.Web.EntitiesDTO.UserDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MUE.Web.EntitiesDTO.BuildingDTO
{
    public class CreateFlatDTO
    {
        [Display(Name = "ID")]
        public Guid FlatId { get; set; }
        [Display(Name = "Номер")]
        public int Number { get; set; }
        [Display(Name = "Комнат")]
        public int Rooms { get; set; }
        [Display(Name = "Проживающих")]
        public int CountResidents { get; set; }
        [Display(Name = "Площадь")]
        public double Square { get; set; }
        [Display(Name = "Дом")]
        public string Building { get; set; }
        [Display(Name = "Улица")]
        public string Street { get; set; }
    }
}