using MUE.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MUE.Web.Controllers
{
    public class SettlementSheetController : Controller
    {
        private readonly SettlementSheetService settlementSheetService = new SettlementSheetService();
        private readonly ServiceBillService serviceBillService = new ServiceBillService();
        private readonly PeriodService periodService = new PeriodService();
        private readonly BuildingService buildingService = new BuildingService();
        // GET: SettlementSheet
        public async Task<ActionResult> Index()
        {
            return View(await settlementSheetService.GetAll());
        }
        public async Task<ActionResult> Details(Guid PeriodId, Guid FlatId)
        {
            var flat = await buildingService.GetFlatDTO(FlatId);
            var period = await periodService.GetPeriodDTO(PeriodId);
            return View(await serviceBillService.GetAll(flat, period));
        }
    }
}