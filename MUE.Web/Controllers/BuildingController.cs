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
    public class BuildingController : Controller
    {
        private readonly BuildingService buildingService = new BuildingService();
        // GET: Building
        public async Task<ActionResult> Index()
        {
            return View(await buildingService.GetBuildings());
        }
        public async Task<ActionResult> Create()
        {
            SelectList StreetNames = new SelectList(await buildingService.GetStreetNames());
            ViewBag.StreetNames = StreetNames;
            return View();
        }
        public async Task<ActionResult> Delete(Guid id)
        {
            return View(await buildingService.GetBuildingDTO(id));
        }
        [HttpPost]
        public async Task<ActionResult> Create(BuildingDTO dto)
        {
            await buildingService.CreateBuilding(dto);
            return RedirectToAction("Index");
        }
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            await buildingService.DeleteBuilding(id);
            return RedirectToAction("Index");
        }
    }
}