using MUE.Web.Entities;
using MUE.Web.EntitiesDTO.MUEDTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MUE.Web.Services
{
    public class MeterReadingService
    {
        private readonly BuildingService buildingService = new BuildingService();
        private readonly PeriodService periodService = new PeriodService();
        private readonly TypeOfServiceService typeOfServiceService = new TypeOfServiceService();
        public async Task Create(MeterReadingDTO dto)
        {
            //var flats = await buildingService.GetFlatDTO(dto.FlatId);
            //var period = await periodService.GetPeriodDTO(dto.PeriodId);
            Guid id = Guid.NewGuid();
            using (MUEContext db = new MUEContext())
            {
                var mr = await GetEntity(dto.FlatId, dto.PeriodId, dto.TypeofServiceId);
                if (mr != null)
                {
                    await Delete(mr.MeterReadingId);
                }
                MeterReading meterReading = new MeterReading {
                    FlatId = dto.FlatId,
                    MeterReadingId = id,
                    PeriodId = dto.PeriodId,
                    TypeofServiceId = dto.TypeofServiceId,
                    Value = dto.Value,
                };
                db.MeterReadings.Add(meterReading);
                await db.SaveChangesAsync();
            }
        }
        public async Task<MeterReadingDTO> GetDTO(Guid id)
        {
            var periods = await periodService.GetPeriods();
            var typeofservices = await typeOfServiceService.GetTypeOfServices();
            var flats = await buildingService.GetFlats();
            using (MUEContext db = new MUEContext())
            {
                return await db.MeterReadings.Where(mr => mr.MeterReadingId == id).Select(mr => new MeterReadingDTO { 
                FlatId = mr.FlatId,
                MeterReadingId = mr.MeterReadingId,
                PeriodId = mr.PeriodId,
                TypeofServiceId = mr.TypeofServiceId,
                CodePeriod = periods.Where(p => p.PeriodId == mr.PeriodId).Select(p => p.Name).FirstOrDefault(),
                Flat = flats.Where(f => f.FlatId == mr.FlatId).Select(f => f.Address).FirstOrDefault(),
                Value = mr.Value,
                TypeOfService = typeofservices.Where(t=> t.TypeOfServiceId == mr.TypeofServiceId).Select(t => t.Name).FirstOrDefault()
                }).FirstOrDefaultAsync();
            }
        }

        public async Task<IEnumerable<MeterReadingDTO>> GetForPeriod(Guid PeriodId)
        {
            var typeofservices = await typeOfServiceService.GetTypeOfServices();
            var flats = await buildingService.GetFlats();
            var period = await periodService.GetPeriodDTO(PeriodId);
            using (MUEContext db = new MUEContext())
            {
                var mrs = await db.MeterReadings.ToListAsync();
                return mrs.Where(mr => mr.PeriodId == period.PeriodId).Select(mr => new MeterReadingDTO
                {
                    FlatId = mr.FlatId,
                    MeterReadingId = mr.MeterReadingId,
                    PeriodId = mr.PeriodId,
                    TypeofServiceId = mr.TypeofServiceId,
                    CodePeriod = period.Name,
                    Flat = flats.Where(f => f.FlatId == mr.FlatId).Select(f => f.Address).FirstOrDefault(),
                    Value = mr.Value,
                    TypeOfService = typeofservices.Where(t => t.TypeOfServiceId == mr.TypeofServiceId).Select(t => t.Name).FirstOrDefault()
                }).ToList();
            }
        }
        private async Task<MeterReading> GetEntity(Guid id) 
        {
            using (MUEContext db = new MUEContext())
            {
                return await db.MeterReadings.Where(mr => mr.MeterReadingId == id).FirstOrDefaultAsync();
            }
        }

        private async Task<MeterReading> GetEntity(Guid FlatId, Guid PeriodId, Guid TypeofServiceId )
        {
            using (MUEContext db = new MUEContext())
            {
                return await db.MeterReadings.Where(mr => mr.FlatId == FlatId && mr.PeriodId == PeriodId && mr.TypeofServiceId == TypeofServiceId).FirstOrDefaultAsync();
            }
        }
        public async Task<IList<MeterReadingDTO>> GetMeterReadings() 
        {
            var periods = await periodService.GetPeriods();
            var typeofservices = await typeOfServiceService.GetTypeOfServices();
            var flats = await buildingService.GetFlats();
            using (MUEContext db = new MUEContext())
            {
                return await db.MeterReadings.Select(mr => new MeterReadingDTO
                {
                    FlatId = mr.FlatId,
                    MeterReadingId = mr.MeterReadingId,
                    PeriodId = mr.PeriodId,
                    TypeofServiceId = mr.TypeofServiceId,
                    CodePeriod = periods.Where(p => p.PeriodId == mr.PeriodId).Select(p => p.Name).FirstOrDefault(),
                    Flat = flats.Where(f => f.FlatId == mr.FlatId).Select(f => f.Address).FirstOrDefault(),
                    Value = mr.Value,
                    TypeOfService = typeofservices.Where(t => t.TypeOfServiceId == mr.TypeofServiceId).Select(t => t.Name).FirstOrDefault()
                }).ToListAsync();
            }
        }
        public async Task Delete(Guid id)
        {          
            using (MUEContext db = new MUEContext())
            {
                var mr = await GetEntity(id);
                db.Entry(mr).State = EntityState.Deleted;
                db.MeterReadings.Remove(mr);
                await db.SaveChangesAsync();
            }
        }

    }
}