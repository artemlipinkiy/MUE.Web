using MUE.Web.Entities;
using MUE.Web.EntitiesDTO.MUEDTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MUE.Web.Services
{
    public class PeriodService
    {
        public async Task CreatePeriod(PeriodDTO dto) 
        {
            Guid id = Guid.NewGuid();
            using (MUEContext db = new MUEContext())
            {
                //TODO: AutoName - low priority
                Period period = new Period 
                {
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate, 
                    IsCurrent = dto.IsCurrent,
                    Name = dto.Name,
                    PeriodId = id
                };
                db.Periods.Add(period);
                await db.SaveChangesAsync();
            }
        }
        public async Task DeletePeriod(Guid id) 
        {
            using (MUEContext db = new MUEContext())
            {
                Period period = await GetPeriodEntity(id);
                db.Entry(period).State = EntityState.Deleted;
                db.Periods.Remove(period);
                await db.SaveChangesAsync();
            }
        }
        public async Task SetCurrent(Guid id, bool Current) 
        {
            var periodentity = await GetPeriodEntity(id);
            using (MUEContext db = new MUEContext())
            {
                periodentity.IsCurrent = Current;
                db.Entry(periodentity).State = EntityState.Modified;

                await db.SaveChangesAsync();
            }
        }
        public async Task UpdatePeriod() 
        { 
        // TODO: Priority 2
        }
        public async Task<PeriodDTO> GetPeriodDTO(Guid id) 
        {
            using (MUEContext db = new MUEContext())
            {
                return await db.Periods.Where(p => p.PeriodId == id).Select(p => new PeriodDTO
                {
                    EndDate = p.EndDate,
                    StartDate = p.StartDate,
                    IsCurrent = p.IsCurrent,
                    Name = p.Name,
                    PeriodId = p.PeriodId
                }).FirstOrDefaultAsync();
            }
        }
        public async Task<PeriodDTO> GetPreviousPerioDTO(Guid id)
        {
            using (MUEContext db = new MUEContext())
            {
                var periods = await db.Periods.OrderBy(t => t.StartDate).ToListAsync();
                for (int i = 1; i < periods.Count; i++)
                {
                    if (periods[i].PeriodId == id)
                    {
                        return new PeriodDTO {
                            EndDate = periods[i-1].EndDate,
                            StartDate = periods[i - 1].StartDate,
                            IsCurrent = periods[i - 1].IsCurrent,
                            Name = periods[i - 1].Name,
                            PeriodId = periods[i - 1].PeriodId
                        };
                    }
                }
                return null;
            }
        }
        private async Task<Period> GetPeriodEntity(Guid id) 
        {
            using (MUEContext db = new MUEContext())
            {
                return await db.Periods.Where(p => p.PeriodId == id).FirstOrDefaultAsync();
            }
        }
        public async Task<IList<PeriodDTO>> GetPeriods()
        {
            using (MUEContext db = new MUEContext())
            {
               return await db.Periods.Select(p => new PeriodDTO
                {
                    EndDate = p.EndDate,
                    StartDate = p.StartDate,
                    IsCurrent = p.IsCurrent,
                    Name = p.Name,
                    PeriodId = p.PeriodId
                }).ToListAsync();
            }
        }
    }
}