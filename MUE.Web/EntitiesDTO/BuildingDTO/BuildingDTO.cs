using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MUE.Web.EntitiesDTO.BuildingDTO
{
    public class BuildingDTO
    {
        [Display(Name = "ID")]
        public Guid BuildingId { get; set; }
        [Display(Name = "Номер")]
        public string Number { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "ID улицы")]
        public Guid? StreetId { get; set; }
        [Display(Name = "Улица")]
        public string StreetName { get; set; }
    }
}