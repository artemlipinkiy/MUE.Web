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
    public class UserController : Controller
    {
        private readonly AccountService accountService = new AccountService();
        // GET: User
        public async Task<ActionResult> Index()
        {
            return View(await accountService.GetAll());
        }
    }
}