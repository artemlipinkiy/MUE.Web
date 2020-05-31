using MUE.Web.EntitiesDTO.MUEDTO;
using MUE.Web.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using MUE.Web.EntitiesDTO.BuildingDTO;
using MUE.Web.EntitiesDTO.UserDTO;

namespace MUE.Web.Services
{
    public class SettlementSheetService
    {
        private readonly BuildingService buildingService = new BuildingService();
        private readonly PeriodService periodService = new PeriodService();
        private readonly ServiceBillService serviceBillService = new ServiceBillService();
        private async Task<SettlementSheet> GetEntity(Guid id) 
        {
            using (MUEContext db = new MUEContext())
            {
                return await db.SettlementSheets.Where(ss => ss.SettlementSheetId == id).FirstOrDefaultAsync();
            }
        }
        public async Task Create(PeriodDTO dto) 
        {
            var flats = await buildingService.GetFlats();
            foreach (var item in flats)
            {
                await Create(item, dto);
            }
        }
        public async Task Create(FlatDTO dto, PeriodDTO periodDTO) 
        {
            var billservice = await serviceBillService.GetAll(dto, periodDTO);
            double summ= 0;
            foreach (var item in billservice)
            {
                summ += item.Summ;
            }
            await Create(new SettlementSheetDTO { 
            AmmountToBePaid = summ,
            FlatId = dto.FlatId,
            PeriodId = periodDTO.PeriodId,
            Status = 0,
            Flat = dto.Address,
            Period = periodDTO.Name,
            });
        }
        public async Task Create(SettlementSheetDTO dto) 
        {
            Guid id = Guid.NewGuid();
            var ReCreate = await Exist(dto.FlatId, dto.PeriodId);
            if (ReCreate != Guid.Empty)
            {
                await Delete(ReCreate);
            }
            using (MUEContext db = new MUEContext())
            {
                db.SettlementSheets.Add(new SettlementSheet { 
                SettlementSheetId = id,
                FlatId = dto.FlatId,
                PeriodId = dto.PeriodId,
                AmmountToBePaid = dto.AmmountToBePaid,
                Description = dto.Description,
                Status = 0,
                });
                await db.SaveChangesAsync();
            }
        }
        public async Task<Guid> Exist(Guid FlatId, Guid PeriodId) 
        {
            using (MUEContext db = new MUEContext())
            {
                var ssdto = await db.SettlementSheets.Where(s => s.FlatId == FlatId && s.PeriodId == PeriodId).FirstOrDefaultAsync();
                if (ssdto == null)
                    return Guid.Empty;
                else
                    return ssdto.SettlementSheetId;
            }
        }
        public async Task Delete(Guid id)
        {
            using (MUEContext db = new MUEContext())
            {
                SettlementSheet SettlementSheet = await GetEntity(id);
                db.Entry(SettlementSheet).State = EntityState.Deleted;
                db.SettlementSheets.Remove(SettlementSheet);
                await db.SaveChangesAsync();
            }
        }
        public async Task<SettlementSheetDTO> Get(Guid id) 
        {
           
            using (MUEContext db = new MUEContext())
            {
                var ss = await db.SettlementSheets.Where(ssd => ssd.SettlementSheetId == id).FirstOrDefaultAsync();
                var flat = await buildingService.GetFlatDTO(ss.FlatId);
                var period = await periodService.GetPeriodDTO(ss.PeriodId);
                return new SettlementSheetDTO {
                AmmountToBePaid = ss.AmmountToBePaid,
                Description = ss.Description,
                FlatId = ss.FlatId,
                PeriodId = ss.PeriodId,
                SettlementSheetId = ss.SettlementSheetId,
                Status = ss.Status,
                Flat = flat.Address,
                Period = period.Name
                };
            }
        }
        public async Task<IEnumerable<SettlementSheetDTO>> GetAll()
        {
            var flats = await buildingService.GetFlats();
            var periods = await periodService.GetPeriods();
            using (MUEContext db = new MUEContext())
            {
                var ss = await db.SettlementSheets.ToListAsync();

                return ss.Select(ssd => new SettlementSheetDTO
                {
                    AmmountToBePaid = ssd.AmmountToBePaid,
                    Description = ssd.Description,
                    FlatId = ssd.FlatId,
                    PeriodId = ssd.PeriodId,
                    SettlementSheetId = ssd.SettlementSheetId,
                    Status = ssd.Status,
                    Flat = flats.Where(f => ssd.FlatId == f.FlatId ).Select(f => f.Address).FirstOrDefault(),
                    Period = periods.Where(p => ssd.PeriodId == p.PeriodId).Select(p => p.Name).FirstOrDefault()
                }).ToList();

            }
        }
        public async Task<IEnumerable<SettlementSheetDTO>> GetAll(PeriodDTO periodDTO)
        {
            var flats = await buildingService.GetFlats();
            var periods = await periodService.GetPeriods();
            using (MUEContext db = new MUEContext())
            {
                var ss = await db.SettlementSheets.Where(sse => sse.PeriodId == periodDTO.PeriodId).ToListAsync();

                return ss.Select(ssd => new SettlementSheetDTO
                {
                    AmmountToBePaid = ssd.AmmountToBePaid,
                    Description = ssd.Description,
                    FlatId = ssd.FlatId,
                    PeriodId = ssd.PeriodId,
                    SettlementSheetId = ssd.SettlementSheetId,
                    Status = ssd.Status,
                    Flat = flats.Where(f => ssd.FlatId == f.FlatId).Select(f => f.Address).FirstOrDefault(),
                    Period = periods.Where(p => ssd.PeriodId == p.PeriodId).Select(p => p.Name).FirstOrDefault()
                }).ToList();

            }
        }
        public async Task<IEnumerable<SettlementSheetDTO>> GetAll(FlatDTO flatDTO)
        {
            var flats = await buildingService.GetFlats();
            var periods = await periodService.GetPeriods();
            using (MUEContext db = new MUEContext())
            {
                var ss = await db.SettlementSheets.Where(sse => sse.FlatId == flatDTO.FlatId).ToListAsync();

                return ss.Select(ssd => new SettlementSheetDTO
                {
                    AmmountToBePaid = ssd.AmmountToBePaid,
                    Description = ssd.Description,
                    FlatId = ssd.FlatId,
                    PeriodId = ssd.PeriodId,
                    SettlementSheetId = ssd.SettlementSheetId,
                    Status = ssd.Status,
                    Flat = flats.Where(f => ssd.FlatId == f.FlatId).Select(f => f.Address).FirstOrDefault(),
                    Period = periods.Where(p => ssd.PeriodId == p.PeriodId).Select(p => p.Name).FirstOrDefault()
                }).ToList();

            }
        }
        public async Task<IEnumerable<SettlementSheetDTO>> GetAll(OwnerDTO OwnerDTO)
        {
            var flats = await buildingService.GetFlats(OwnerDTO.OwnerId);
            var periods = await periodService.GetPeriods();
            using (MUEContext db = new MUEContext())
            {
                var ss = await db.SettlementSheets.ToListAsync();
                ss = ss.Where(sse => flats.Select(f => f.FlatId).ToList().Contains(sse.FlatId)).ToList();

                return ss.Select(ssd => new SettlementSheetDTO
                {
                    AmmountToBePaid = ssd.AmmountToBePaid,
                    Description = ssd.Description,
                    FlatId = ssd.FlatId,
                    PeriodId = ssd.PeriodId,
                    SettlementSheetId = ssd.SettlementSheetId,
                    Status = ssd.Status,
                    Flat = flats.Where(f => ssd.FlatId == f.FlatId).Select(f => f.Address).FirstOrDefault(),
                    Period = periods.Where(p => ssd.PeriodId == p.PeriodId).Select(p => p.Name).FirstOrDefault()
                }).ToList();

            }
        }
    }
}