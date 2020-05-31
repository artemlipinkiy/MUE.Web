using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MUE.Web.EntitiesDTO.MUEDTO
{
    public class PeriodDTO
    {
        [Display(Name = "ID")]
        public Guid PeriodId { get; set; }

        [Display(Name = "Код периода")]
        public string Name { get; set; }

        [Display(Name = "Начало")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Конец")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Текущий")]
        public bool IsCurrent { get; set; }
    }
}