using MUE.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MUE.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AccountService accountService = new AccountService();
        // GET: Profile
        public async Task<ActionResult> MyProfile()
        {
            var login = User.Identity.Name;
            if (String.IsNullOrEmpty(login))
            {
                return RedirectToAction("Index", "Home");
            }
            return View(await accountService.MyProfile(login));
        }
    }
}