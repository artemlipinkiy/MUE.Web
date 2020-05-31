using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MUE.Web.EntitiesDTO.UserDTO
{
    public class UserDTO
    {
        [Display(Name ="ID")]
        public Guid UserId { get; set; }
        [Display(Name = "Логин")]
        public string Login { get; set; }
        [Display(Name = "ID Роль")]
        public Guid RoleId { get; set; }
        [Display(Name = "Роль")]
        public string Role { get; set; }
    }
}