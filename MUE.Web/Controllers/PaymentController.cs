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
    public class PaymentController : Controller
    {
        private readonly BuildingService buildingService = new BuildingService();
        private readonly OwnerService ownerService = new OwnerService();
        private readonly AccountService accountService = new AccountService();
        private readonly PeriodService periodService = new PeriodService();
        private readonly TariffService tariffService = new TariffService();
        private readonly TypeOfServiceService typeOfServiceService = new TypeOfServiceService();
        private readonly MeterReadingService meterReadingService = new MeterReadingService();
        private readonly ServiceBillService serviceBillService = new ServiceBillService();
        private readonly SettlementSheetService settlementSheetService = new SettlementSheetService();
        // GET: Payment
        public async Task<ActionResult> MySettlementSheet() 
        {
            var user = await accountService.GetUserId(User.Identity.Name);
            var ownerId = await accountService.GetOwnerId(user);
            var owner = await ownerService.GetDTO(ownerId);
            var SettlementSheet = await settlementSheetService.GetAll(owner);
            return View(SettlementSheet);
        }
        public async Task<ActionResult> FlatSettlementSheet(Guid FlatId) 
        {
            var flat = await buildingService.GetFlatDTO(FlatId);
            var settlementsheetservice = await settlementSheetService.GetAll(flat);
            return View(settlementsheetservice);
        }
        public async Task<ActionResult> Details(Guid FlatId, Guid PeriodId)
        {
            var flat = await buildingService.GetFlatDTO(FlatId);
            var period = await periodService.GetPeriodDTO(PeriodId);
            var details = await serviceBillService.GetAll(flat, period);
            return View(details);
        }
        public async Task<ActionResult> MyAllTariffs() 
        {
            var userId = await accountService.GetUserId(User.Identity.Name);
            var ownerId = await accountService.GetOwnerId(userId);
            return View(await tariffService.GetAllMyTariffs(ownerId));
        }
        public async Task<ActionResult> MyFlats()
        {
            var userID = await accountService.GetUserId(User.Identity.Name);
            var ownerid = await accountService.GetOwnerId(userID);
            if (ownerid == null)
            {
                return RedirectToAction("Index","Home");
            }
            return View(await buildingService.GetFlats(ownerid));
        }
        public async Task<ActionResult> SelectPeriod(Guid FlatId) 
        {
            ViewBag.FlatId = FlatId;
            return View(await periodService.GetPeriods());
        }
        public async Task<ActionResult> MyTariffs(Guid FlatId, Guid PeriodId) 
        {
            ViewBag.FlatId = FlatId;
            ViewBag.PeriodId = PeriodId;
            return View(await tariffService.GetMyTariffs(FlatId));
        }
        //Форма заполнения данных счетчика
        public async Task<ActionResult> SubmitMeterReading(Guid FlatId, Guid PeriodId, Guid TariffId) 
        {
            ViewBag.FlatId = FlatId;
            ViewBag.PeriodId = PeriodId;
            ViewBag.TariffId = TariffId;
            var tariff = await tariffService.GetTariffDTO(TariffId);
            return View(new MeterReadingDTO { FlatId = FlatId, PeriodId = PeriodId, TypeofServiceId =  tariff.TypeOfServiceId});
        }
        [HttpPost]
        public async Task<ActionResult> SubmitMeterReading(MeterReadingDTO dto)
        {
            await meterReadingService.Create(dto);
            return RedirectToAction("MyFlats");
        }
    }
}