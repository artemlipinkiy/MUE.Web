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
    public class TariffController : Controller
    {
        private readonly TariffService tariffService = new TariffService();
        // GET: Tariff
        public async Task<ActionResult> Index()
        {
            return View(await tariffService.GetTariffs());
        }
        //public async Task<ActionResult> Create()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<ActionResult> Create(CreateTariffDTO dto)
        //{
        //    await tariffService.CreateTariff(dto);
        //    return RedirectToAction("Index");
        //}
        public async Task<ActionResult> Delete(Guid id)
        {
            return View(await tariffService.GetTariffDTO(id));
        }
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            await tariffService.DeleteTariff(id);
            return RedirectToAction("Index");
        }
    }
}