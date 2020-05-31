using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MUE.Web.EntitiesDTO.UserDTO
{
    public class CreateOwnerDTO
    {
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Display(Name = "Отчество")]
        public string MiddlleName { get; set; }
        [Display(Name = "Статус")]
        public string Status { get; set; }
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        [Display(Name = "Повторите пароль")]
        public string ConfirmPassword { get; set; }
    }
}