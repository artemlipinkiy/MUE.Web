using MUE.Web.EntitiesDTO.BuildingDTO;
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
    public class FlatController : Controller
    {
        private readonly SettlementSheetService settlementSheetService = new SettlementSheetService();
        private readonly BuildingService buildingService = new BuildingService();
        private readonly TypeOfServiceService typeOfServiceService = new TypeOfServiceService();
        private readonly TariffService tariffService = new TariffService();
        private readonly OwnerService ownerService = new OwnerService();
        // GET: Flat
        public async Task<ActionResult> Index()
        {
            return View(await buildingService.GetFlats());
        }
        public async Task<ActionResult> SelectSheet(Guid FlatId) 
        {
            var flat = await buildingService.GetFlatDTO(FlatId);
            return View(await settlementSheetService.GetAll(flat));
        }
        public async Task<ActionResult> SelectOwner(Guid flatId)
        {
            ViewBag.flatId = flatId;
            var flat = await buildingService.GetFlatDTO(flatId);
            ViewBag.HasOwner = false;
            if (flat.OwnersId.HasValue)
                ViewBag.HasOwner = true;
            return View(await ownerService.GetOwners());
        }
        public async Task<ActionResult> ConfirmOwner(Guid OwnerId, Guid FlatId)
        {
            await buildingService.SetFlatOwner(FlatId,OwnerId);
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> DeleteOwner(Guid FlatId)
        {
            await buildingService.SetFlatOwner(FlatId, null);
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> SelectService(Guid flatId)
        {
            ViewBag.flatId = flatId;
            return View(await typeOfServiceService.GetTypeOfServices());
        }
        public async Task<ActionResult> SelectTariff(Guid flatId, Guid serviceId)
        {
            var tariff = await tariffService.GetTariffDTO(flatId, serviceId);
            if (tariff != null)
            {
                return View(tariff);
            }
            ViewBag.flatId = flatId;
            ViewBag.serviceId = serviceId;
            return View(new TariffDTO {TariffId = flatId, TypeOfServiceId = serviceId });
        }
        [HttpPost, ActionName("SelectTariff")]
        public async Task<ActionResult> ConfirmTariff(TariffDTO dto)
        {
            var tariff = await tariffService.GetTariffDTO(dto.FlatId, dto.TypeOfServiceId);
            if (tariff!= null)
            {
                await tariffService.DeleteTariff(tariff.TariffId);
            }
            await tariffService.CreateTariff(dto);
            return RedirectToAction("SelectService", new {flatId = dto.FlatId});
        }
        public async Task<ActionResult> Create()
        {
            SelectList StreetNames = new SelectList(await buildingService.GetStreetNames());
            ViewBag.StreetNames = StreetNames;
            SelectList BuildingNames = new SelectList(await buildingService.GetBuildingNames());
            ViewBag.BuildingNames = BuildingNames;
            return View();
        }
        public async Task<ActionResult> Delete(Guid id)
        {
            return View(await buildingService.GetFlatDTO(id));
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateFlatDTO dto)
        {
            await buildingService.CreateFlat(dto);
            return RedirectToAction("Index");
        }
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            await buildingService.DeleteFlat(id);
            return RedirectToAction("Index");
        }
    }
}