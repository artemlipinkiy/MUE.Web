using MUE.Web.EntitiesDTO.BuildingDTO;
using MUE.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MUE.Web.Controllers
{
    public class StreetController : Controller
    {
        // GET: Street
        private readonly BuildingService buildingService = new BuildingService();
        public async Task<ActionResult> Streets()
        {
            return View(await buildingService.GetStreets());
        }
        public async Task<ActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(StreetDTO dto)
        {
            await buildingService.CreateStreet(dto);
            return RedirectToAction("Streets", "Street");
        }
        public async Task<ActionResult> Delete (Guid id)
        {
            return View(await buildingService.GetStreetDTO(id));
        }
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            await buildingService.DeleteStreet(id);
            return RedirectToAction("Streets", "Street");
        }
        
    }
}