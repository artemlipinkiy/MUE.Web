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
    public class PeriodController : Controller
    {
        private readonly MeterReadingService meterReadingService = new MeterReadingService();
        private readonly PeriodService periodService = new PeriodService();
        private readonly BuildingService buildingService = new BuildingService();
        private readonly ServiceBillService serviceBillService = new ServiceBillService();
        private readonly TariffService tariffService = new TariffService();
        private readonly TypeOfServiceService typeOfServiceService = new TypeOfServiceService();
        private readonly SettlementSheetService settlementSheetService = new SettlementSheetService();
        // GET: Period
        public async Task<ActionResult> Index()
        {
            return View(await periodService.GetPeriods());
        }
        public async Task<ActionResult> Create()
        {
            return View();
        }
        public async Task<ActionResult> Delete(Guid id)
        {
            return View(await periodService.GetPeriodDTO(id));
        }
        [HttpPost]
        public async Task<ActionResult> Create(PeriodDTO dto)
        {
            await periodService.CreatePeriod(dto);
            return RedirectToAction("Index");
        }
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            await periodService.DeletePeriod(id);
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> CalculateForThePeriod(Guid PeriodId)
        {
            var period = await periodService.GetPeriodDTO(PeriodId);
            await serviceBillService.Create(period);
            await settlementSheetService.Create(period);
            await periodService.SetCurrent(period.PeriodId, false);
            return RedirectToAction("Index","Home");
        }
        public async Task<ActionResult> SelectFlat(Guid PeriodId) 
        {
            ViewBag.PeriodId = PeriodId;
            var flats =  await buildingService.GetFlats();
            return View(flats);
        }
        public async Task<ActionResult> Tariffs(Guid PeriodId, Guid FlatId) 
        {
            ViewBag.FlatId = FlatId;
            ViewBag.PeriodId = PeriodId;
            return View(await tariffService.GetMyTariffs(FlatId));
        }

        //Форма заполнения 
        public async Task<ActionResult> SubmitMeterReading(Guid FlatId, Guid PeriodId, Guid TariffId)
        {
            var tariff = await tariffService.GetTariffDTO(TariffId);
            return View(new MeterReadingDTO { FlatId = FlatId, PeriodId = PeriodId, TypeofServiceId = tariff.TypeOfServiceId });
        }
        [HttpPost]
        public async Task<ActionResult> SubmitMeterReading(MeterReadingDTO dto)
        {
            await meterReadingService.Create(dto);
            return RedirectToAction("Index");
        }
    }
}