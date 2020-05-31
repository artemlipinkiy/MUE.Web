using MUE.Web.Entities;
using MUE.Web.EntitiesDTO.BuildingDTO;
using MUE.Web.EntitiesDTO.MUEDTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MUE.Web.Services
{
    public class ServiceBillService
    {
        private readonly BuildingService buildingService = new BuildingService();
        private readonly TariffService tariffService = new TariffService();
        private readonly MeterReadingService meterReadingService = new MeterReadingService();
        private readonly PeriodService periodService = new PeriodService();
        private readonly TypeOfServiceService typeOfServiceService = new TypeOfServiceService();

        public async Task Create(PeriodDTO dto) 
        {
            var flats = await buildingService.GetFlats();
            foreach (var item in flats)
            {
                await Create(item, dto.PeriodId);
            }
        }
        public async Task Create(FlatDTO dto, Guid PeriodId)
        {
            var period = await periodService.GetPeriodDTO(PeriodId);
            var previousPeriod = await periodService.GetPreviousPerioDTO(PeriodId);
            var meterReadings = await meterReadingService.GetForPeriod(PeriodId);
            IEnumerable<MeterReadingDTO> previousMeterReadings;
            if (previousPeriod == null)
                previousMeterReadings = null;
            else
                previousMeterReadings = await meterReadingService.GetForPeriod(previousPeriod.PeriodId);

            var tariffs = await tariffService.GetTariffs();
            var tariffForFLat = tariffs.Where(t => t.FlatId == dto.FlatId).ToList();
            
           
            foreach (var item in tariffForFLat)
            {
                if (item.IsMeter)
                {
                    var CurrentValue = meterReadings.Where(m => m.FlatId == dto.FlatId && m.TypeofServiceId == item.TypeOfServiceId).FirstOrDefault();
                    MeterReadingDTO LastValue;
                    if (previousMeterReadings == null)
                        LastValue = null;
                    else
                        LastValue = previousMeterReadings.Where(m => m.FlatId == dto.FlatId && m.TypeofServiceId == item.TypeOfServiceId).FirstOrDefault();
                    if (LastValue == null)
                    {
                        if (CurrentValue == null)
                        {

                            await Create(new ServiceBillDTO
                            {
                                Summ = 0,
                                FlatId = dto.FlatId,
                                PeriodId = PeriodId,
                                TypeOfServiceId = item.TypeOfServiceId
                            });
                        }
                        else
                        {
                            await Create(new ServiceBillDTO
                            {
                                Summ = CurrentValue.Value * item.Value,
                                FlatId = dto.FlatId,
                                PeriodId = PeriodId,
                                TypeOfServiceId = item.TypeOfServiceId
                            });
                        }
                    }
                    else
                    {
                        await Create(new ServiceBillDTO
                        {
                            Summ = LastValue.Value > CurrentValue.Value? 0 : (CurrentValue.Value - LastValue.Value) * item.Value,
                            FlatId = dto.FlatId,
                            PeriodId = PeriodId,
                            TypeOfServiceId = item.TypeOfServiceId
                        });
                    }
                }
                else
                {
                    var CurrentValue = meterReadings.Where(m => m.FlatId == dto.FlatId && m.TypeofServiceId == item.TypeOfServiceId).FirstOrDefault();
                    if (CurrentValue == null)
                    {
                        await Create(new ServiceBillDTO
                        {
                            Summ = 0,
                            FlatId = dto.FlatId,
                            PeriodId = PeriodId,
                            TypeOfServiceId = item.TypeOfServiceId
                        });
                    }
                    else
                        await Create(new ServiceBillDTO
                        {
                            Summ = (CurrentValue.Value) * item.Value,
                            FlatId = dto.FlatId,
                            PeriodId = PeriodId,
                            TypeOfServiceId = item.TypeOfServiceId
                        });
                }
            }
        }
        public async Task Create(ServiceBillDTO dto) 
        {
            Guid id = Guid.NewGuid();
            var ReCreate = await Exist(dto);
            if (ReCreate != Guid.Empty)
            {
                await Delete(ReCreate);
            }
            using (MUEContext db = new MUEContext())
            { 
                ServiceBill serviceBill = new ServiceBill { 
                ServiceBillId = id,
                Status = 0,
                FlatId = dto.FlatId,
                PeriodId = dto.PeriodId,
                TypeOfServiceId = dto.TypeOfServiceId,
                Summ = dto.Summ
                };
                db.ServiceBills.Add(serviceBill);
                await db.SaveChangesAsync();
            }
        }
        private async Task<ServiceBill> GetEntity(Guid id)
        {
            using (MUEContext db = new MUEContext())
            {
                return await db.ServiceBills.Where(sb => sb.ServiceBillId == id).FirstOrDefaultAsync();
            }
        }
        public async Task Delete(Guid id)
        {
            using (MUEContext db = new MUEContext())
            {
                ServiceBill ServiceBill = await GetEntity(id);
                db.Entry(ServiceBill).State = EntityState.Deleted;
                db.ServiceBills.Remove(ServiceBill);
                await db.SaveChangesAsync();
            }
        }
        public async Task<Guid> Exist(ServiceBillDTO dto) 
        {
            using (MUEContext db = new MUEContext())
            {
                var ServiceBill = await db.ServiceBills
                    .Where(sb => sb.FlatId == dto.FlatId && sb.PeriodId == dto.PeriodId && sb.TypeOfServiceId == dto.TypeOfServiceId).FirstOrDefaultAsync();
                if (ServiceBill == null)
                    return Guid.Empty;
                else return ServiceBill.ServiceBillId;
            }
        }
        public async Task<ServiceBillDTO> GetDTO(Guid id) 
        {
            var flats = await buildingService.GetFlats();
            var periods = await periodService.GetPeriods();
            var typeofservices = await typeOfServiceService.GetTypeOfServices();

            using (MUEContext db = new MUEContext())
            {
                var servicebill = await db.ServiceBills.Where(s => s.ServiceBillId == id).FirstOrDefaultAsync();

                return new ServiceBillDTO {
                    FlatId = servicebill.FlatId,
                    PeriodId = servicebill.PeriodId,
                    ServiceBillId = servicebill.ServiceBillId,
                    TypeOfServiceId = servicebill.TypeOfServiceId,
                    Summ = servicebill.Summ,
                    Status = servicebill.Status,
                    Period = periods.Where(p => p.PeriodId == servicebill.PeriodId).Select(p => p.Name).FirstOrDefault(),
                    Flat = flats.Where(f => f.FlatId == servicebill.FlatId).Select(f => f.Address).FirstOrDefault(), 
                    TypeOfService = typeofservices.Where(t => t.TypeOfServiceId == servicebill.TypeOfServiceId).Select(t => t.Name).FirstOrDefault()
                };
            }
        }
        public async Task<IEnumerable<ServiceBillDTO>> GetAll()
        {
            var flats = await buildingService.GetFlats();
            var periods = await periodService.GetPeriods();
            var typeofservices = await typeOfServiceService.GetTypeOfServices();

            using (MUEContext db = new MUEContext())
            {
                var servicebills = await db.ServiceBills.ToListAsync();

                return servicebills.Select( s => new ServiceBillDTO
                {
                    FlatId = s.FlatId,
                    PeriodId = s.PeriodId,
                    ServiceBillId = s.ServiceBillId,
                    TypeOfServiceId = s.TypeOfServiceId,
                    Summ = s.Summ,
                    Status = s.Status,
                    Period = periods.Where(p => p.PeriodId == s.PeriodId).Select(p => p.Name).FirstOrDefault(),
                    Flat = flats.Where(f => f.FlatId == s.FlatId).Select(f => f.Address).FirstOrDefault(),
                    TypeOfService = typeofservices.Where(t => t.TypeOfServiceId == s.TypeOfServiceId).Select(t => t.Name).FirstOrDefault()
                }).ToList();
            }
        }
        public async Task<IEnumerable<ServiceBillDTO>> GetAll(PeriodDTO dto)
        {
            var flats = await buildingService.GetFlats();
            var periods = await periodService.GetPeriods();
            var typeofservices = await typeOfServiceService.GetTypeOfServices();

            using (MUEContext db = new MUEContext())
            {
                var servicebills = await db.ServiceBills.Where(s => s.PeriodId == dto.PeriodId).ToListAsync();

                return servicebills.Select(s => new ServiceBillDTO
                {
                    FlatId = s.FlatId,
                    PeriodId = s.PeriodId,
                    ServiceBillId = s.ServiceBillId,
                    TypeOfServiceId = s.TypeOfServiceId,
                    Summ = s.Summ,
                    Status = s.Status,
                    Period = periods.Where(p => p.PeriodId == s.PeriodId).Select(p => p.Name).FirstOrDefault(),
                    Flat = flats.Where(f => f.FlatId == s.FlatId).Select(f => f.Address).FirstOrDefault(),
                    TypeOfService = typeofservices.Where(t => t.TypeOfServiceId == s.TypeOfServiceId).Select(t => t.Name).FirstOrDefault()
                }).ToList();
            }
        }
        public async Task<IEnumerable<ServiceBillDTO>> GetAll(FlatDTO dto)
        {
            var flats = await buildingService.GetFlats();
            var periods = await periodService.GetPeriods();
            var typeofservices = await typeOfServiceService.GetTypeOfServices();

            using (MUEContext db = new MUEContext())
            {
                var servicebills = await db.ServiceBills.Where(s => s.FlatId == dto.FlatId).ToListAsync();

                return servicebills.Select(s => new ServiceBillDTO
                {
                    FlatId = s.FlatId,
                    PeriodId = s.PeriodId,
                    ServiceBillId = s.ServiceBillId,
                    TypeOfServiceId = s.TypeOfServiceId,
                    Summ = s.Summ,
                    Status = s.Status,
                    Period = periods.Where(p => p.PeriodId == s.PeriodId).Select(p => p.Name).FirstOrDefault(),
                    Flat = flats.Where(f => f.FlatId == s.FlatId).Select(f => f.Address).FirstOrDefault(),
                    TypeOfService = typeofservices.Where(t => t.TypeOfServiceId == s.TypeOfServiceId).Select(t => t.Name).FirstOrDefault()
                }).ToList();
            }
        }
        public async Task<IEnumerable<ServiceBillDTO>> GetAll(FlatDTO dto, PeriodDTO periodDTO)
        {
            var flats = await buildingService.GetFlats();
            var periods = await periodService.GetPeriods();
            var typeofservices = await typeOfServiceService.GetTypeOfServices();

            using (MUEContext db = new MUEContext())
            {
                var servicebills = await db.ServiceBills.Where(s => s.FlatId == dto.FlatId && s.PeriodId == periodDTO.PeriodId).ToListAsync();

                return servicebills.Select(s => new ServiceBillDTO
                {
                    FlatId = s.FlatId,
                    PeriodId = s.PeriodId,
                    ServiceBillId = s.ServiceBillId,
                    TypeOfServiceId = s.TypeOfServiceId,
                    Summ = s.Summ,
                    Status = s.Status,
                    Period = periods.Where(p => p.PeriodId == s.PeriodId).Select(p => p.Name).FirstOrDefault(),
                    Flat = flats.Where(f => f.FlatId == s.FlatId).Select(f => f.Address).FirstOrDefault(),
                    TypeOfService = typeofservices.Where(t => t.TypeOfServiceId == s.TypeOfServiceId).Select(t => t.Name).FirstOrDefault()
                }).ToList();
            }
        }

    }
}