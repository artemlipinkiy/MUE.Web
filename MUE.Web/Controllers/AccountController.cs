using Microsoft.Owin.Security;
using MUE.Web.EntitiesDTO.UserDTO;
using MUE.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MUE.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountService userService = new AccountService();
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        // GET: Account
        public async Task<ActionResult> CreateAdm() { return View(); }
        [HttpPost]
        public async Task<ActionResult> CreateAdm(CreateUserDTO dto) 
        {
            var result = await userService.CreateAdm(dto);
            if (result)
            {
               return RedirectToAction("Index","User");
            }
            return View();
        }
        public async Task<ActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Register(CreateUserDTO dto)
        {
            var register = await userService.Register(dto);
            if (register)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        public async Task<ActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(LoginUserDTO dto)
        {
            var login = await userService.Login(dto, AuthenticationManager);
            if (login)
            {
                return RedirectToAction("Index", "Home");
            }
            else return View();
        }
        public async Task<ActionResult> Logout() 
        {
            await userService.Logout(AuthenticationManager);
            return RedirectToAction("Index","Home");
        }
    }
}