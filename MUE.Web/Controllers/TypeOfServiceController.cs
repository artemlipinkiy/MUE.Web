using MUE.Web.EntitiesDTO.MUEDTO;
using MUE.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MUE.Web.Controllers
{
    public class TypeOfServiceController : Controller
    {
        private readonly TypeOfServiceService typeOfServiceService = new TypeOfServiceService();
        // GET: TypeOfService
        public async Task<ActionResult> Index()
        {
            return View(await typeOfServiceService.GetTypeOfServices());
        }
        public async Task<ActionResult> Create() 
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(TypeOfServiceDTO dto)
        {
            await typeOfServiceService.CreateTypeOfService(dto);
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Delete(Guid id)
        {
            return View(await typeOfServiceService.GetTypeOfServiceDTO(id));
        }
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            await typeOfServiceService.DeleteTypeOfService(id);
            return RedirectToAction("Index");
        }
    }
}