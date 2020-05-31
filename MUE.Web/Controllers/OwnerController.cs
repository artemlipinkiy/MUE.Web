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
    public class OwnerController : Controller
    {
        private readonly OwnerService ownerService = new OwnerService();
        private readonly AccountService accountService = new AccountService();
        // GET: Owner
        public async Task<ActionResult> Index()
        {
            return View(await ownerService.GetOwners());
        }

        // GET: Owner/Create
        public async Task<ActionResult> Create()
        {
            return View();
        }

        // POST: Owner/Create
        [HttpPost]
        public async Task<ActionResult> Create(CreateOwnerDTO dto)
        {
            await accountService.CreateOwner(dto);
            return RedirectToAction("Index");
        }

        // GET: Owner/Edit/5
        public async Task<ActionResult> Edit(Guid id)
        {
            return View(await ownerService.GetDTO(id));
        }

        // POST: Owner/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(OwnerDTO dto)
        {
            await ownerService.Update(dto);
            return RedirectToAction("Index");
        }

        // GET: Owner/Delete/5
        public async Task<ActionResult> Delete(Guid id)
        {
            return View(await ownerService.GetDTO(id));
        }

        // POST: Owner/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirm(Guid id)
        {
            await ownerService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
