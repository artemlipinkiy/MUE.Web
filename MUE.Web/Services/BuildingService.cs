using MUE.Web.Entities;
using MUE.Web.EntitiesDTO.BuildingDTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MUE.Web.Services
{
    public class BuildingService
    {
        private readonly OwnerService ownerService = new OwnerService();
        public async Task CreateStreet(StreetDTO dto) 
        {
            Guid id = Guid.NewGuid();
            using (MUEContext db = new MUEContext())
            {
                Street street = new Street {
                    StreetId = id,
                    Name = dto.Name,
                    Description = dto.Description
                };
                db.Streets.Add(street);
                await db.SaveChangesAsync();
            }
        }
        public async Task CreateBuilding(BuildingDTO dto)
        {
            Guid id = Guid.NewGuid();
            Guid streetId = (await GetStreetEntity(dto.StreetName)).StreetId;
            using (MUEContext db = new MUEContext())
            {
                Building building = new Building
                {
                    BuildingId = id,
                    Description = dto.Description,
                    Number = dto.Number,
                    StreetId = streetId
                };
                db.Buildings.Add(building);
                await db.SaveChangesAsync();
            }
        }

        public async Task CreateBuilding(BuildingDTO dto, Guid StreetId)
        {
            Guid id = Guid.NewGuid();
            using (MUEContext db = new MUEContext())
            {
                Building building = new Building
                {
                    BuildingId = id,
                    Description = dto.Description,
                    Number = dto.Number,
                    StreetId = StreetId
                };
                db.Buildings.Add(building);
                await db.SaveChangesAsync();
            }
        }
        public async Task CreateFlat(CreateFlatDTO dto) 
        {
            Guid id = Guid.NewGuid();
            var streetId = (await GetStreetEntity(dto.Street)).StreetId;
            Building building = await GetBuildingEntity(dto.Building, streetId);
            if (building == null)
            {
               await CreateBuilding(new BuildingDTO { Number = dto.Building}, streetId);
            }
            building = await GetBuildingEntity(dto.Building, streetId);
            var DontCreate = await Exist(building.BuildingId, dto.Number);
            if (DontCreate != Guid.Empty)
                return;
            using (MUEContext db = new MUEContext())
            {
                Flat flat = new Flat {
                    Square = dto.Square,
                    Rooms = dto.Rooms,
                    CountResidents = dto.CountResidents,
                    FlatId = id,
                    Number = dto.Number,
                    BuildingId = building.BuildingId
                };
                db.Flats.Add(flat);
                await db.SaveChangesAsync();
            }
        }
        public async Task<Guid> Exist(Guid BuildingId, int Number) 
        {
            using (MUEContext db = new MUEContext())
            {
                var flat = await db.Flats.Where(f => f.BuildingId == BuildingId && f.Number == Number).FirstOrDefaultAsync();
                if (flat == null)
                    return Guid.Empty;
                else return flat.FlatId;
            }
        }

        private async Task<Street> GetStreetEntity(string Name) 
        {
            using (MUEContext db = new MUEContext())
            {
                return await db.Streets.Where(s => s.Name == Name).FirstOrDefaultAsync();
            }
        }
        private async Task<Street> GetStreetEntity(Guid id)
        {
            using (MUEContext db = new MUEContext())
            {
                return await db.Streets.Where(s => s.StreetId == id).FirstOrDefaultAsync();
            }
        }
        public async Task<StreetDTO> GetStreetDTO(Guid id) 
        {
            using (MUEContext db = new MUEContext())
            {
                return await db.Streets.Where(s => s.StreetId == id).Select(s => new StreetDTO
                {
                    StreetId = s.StreetId,
                    Name = s.Name,
                    Description = s.Description
                }).FirstOrDefaultAsync();
            }
        }
        public async Task<IList<string>> GetStreetNames() 
        {
            using (MUEContext db = new MUEContext())
            {
                return await db.Streets.Select(s => s.Name).ToListAsync();
            }
        }

        private async Task<Building> GetBuildingEntity(Guid id)
        {
            using (MUEContext db = new MUEContext())
            {
                return await db.Buildings.Where(b => b.BuildingId == id).FirstOrDefaultAsync();
            }
        }
        private async Task<Building> GetBuildingEntity(string NumberBuilding, Guid streetId)
        {
            using (MUEContext db = new MUEContext())
            {
                return await db.Buildings.Where(b => b.StreetId == streetId && b.Number == NumberBuilding).FirstOrDefaultAsync();
            }
        }
        public async Task<BuildingDTO> GetBuildingDTO(Guid id) 
        {
            using (MUEContext db = new MUEContext())
            {
                var streets = await GetStreets();
                var building = await db.Buildings.Where(b => b.BuildingId == id).FirstOrDefaultAsync();
                return new BuildingDTO 
                {
                    BuildingId = building.BuildingId,
                    Description = building.Description,
                    Number = building.Number,
                    StreetName = streets.Where(s => s.StreetId == building.StreetId).FirstOrDefault().Name,
                    StreetId = building.StreetId
                };
                
              
            }
        }
        public async Task<IList<string>> GetBuildingNames() 
        {
            using (MUEContext db = new MUEContext())
            {
                return await db.Buildings.Select(b => b.Number).Distinct().ToListAsync();
            }
        }

        private async Task<Flat> GetFlatEntity(Guid id)
        {
            using (MUEContext db = new MUEContext())
            {
                return await db.Flats.Where(f => f.FlatId == id).FirstOrDefaultAsync();
            }
        }
        public async Task<FlatDTO> GetFlatDTO(Guid id)
        {
            var buildings = await GetBuildings();
            using (MUEContext db = new MUEContext())
            {
                var flat = await db.Flats.Where(f => f.FlatId == id).FirstOrDefaultAsync();
                return new FlatDTO {
                    Square = flat.Square,
                    Rooms = flat.Rooms,
                    BuildingId = flat.BuildingId,
                    OwnersId = flat.OwnersId,
                    FlatId = flat.FlatId,
                    CountResidents = flat.CountResidents,
                    Address = string.Format("Улица: {0} Дом: {1} Квартира: {2}",
                buildings.Where(b => b.BuildingId == flat.BuildingId).FirstOrDefault().StreetName,
                buildings.Where(b => b.BuildingId == flat.BuildingId).FirstOrDefault().Number.ToString(),
                 flat.Number.ToString())
                };
            }
        }

        public async Task<IList<StreetDTO>> GetStreets() 
        {
            using (MUEContext db = new MUEContext())
            {
                return await db.Streets.Select(s => new StreetDTO
                {
                    StreetId = s.StreetId,
                    Name = s.Name,
                    Description = s.Description
                }).ToListAsync();
            }
        }
        public async Task<IList<BuildingDTO>> GetBuildings()
        {
            var streets = await GetStreets();
            using (MUEContext db = new MUEContext())
            {
                var buildings = await db.Buildings.ToListAsync();
                var result = buildings.Select(b => new BuildingDTO
                {

                    BuildingId = b.BuildingId,
                    Description = b.Description,
                    Number = b.Number,
                    StreetName = streets.Where(s => s.StreetId == b.StreetId).FirstOrDefault().Name,
                    StreetId = b.StreetId
                }).ToList();
                return result;
            }
        }
        public async Task<IList<FlatDTO>> GetFlats() 
        {
            var owners = await ownerService.GetOwners();
            var buildings = await GetBuildings();
            using (MUEContext db = new MUEContext())
            {
                var flats = await db.Flats.ToListAsync();
                return flats.Select(f => new FlatDTO
                {
                    Square = f.Square,
                    Rooms = f.Rooms,
                    Number = f.Number,
                    BuildingId = f.BuildingId,
                    OwnersId = f.OwnersId,
                    FlatId = f.FlatId,
                    OwnersFullName = f.OwnersId.HasValue? owners.Where(o => o.OwnerId == f.OwnersId).Select(o => string.Format("{0} {1} {2}", o.LastName, o.FirstName, o.MiddlleName)).FirstOrDefault() : "",
                    CountResidents = f.CountResidents,
                    Address = string.Format("Улица: {0} Дом: {1} Квартира: {2}",
                buildings.Where(b => b.BuildingId == f.BuildingId).FirstOrDefault().StreetName,
                buildings.Where(b => b.BuildingId == f.BuildingId).FirstOrDefault().Number.ToString(),
                f.Number.ToString())
                }).ToList();
            }
        }
        public async Task<IList<FlatDTO>> GetFlats(Guid ownerId)
        {
            var owners = await ownerService.GetOwners();
            var buildings = await GetBuildings();
            using (MUEContext db = new MUEContext())
            {
                var flats = await db.Flats.ToListAsync();
                return flats.Where(f => f.OwnersId == ownerId).Select(f => new FlatDTO
                {
                    Square = f.Square,
                    Rooms = f.Rooms,
                    Number = f.Number,
                    BuildingId = f.BuildingId,
                    OwnersId = f.OwnersId,
                    FlatId = f.FlatId,
                    OwnersFullName = f.OwnersId.HasValue ? owners.Where(o => o.OwnerId == f.OwnersId).Select(o => o.FirstName + o.LastName + o.MiddlleName).FirstOrDefault() : "",
                    CountResidents = f.CountResidents,
                    Address = string.Format("Улица: {0} Дом: {1} Квартира: {2}",
                buildings.Where(b => b.BuildingId == f.BuildingId).FirstOrDefault().StreetName,
                buildings.Where(b => b.BuildingId == f.BuildingId).FirstOrDefault().Number.ToString(),
                 f.Number.ToString())
                }).ToList();
            }
        }

        public async Task DeleteStreet(Guid id) 
        {
            using (MUEContext db = new MUEContext())
            {
                Street street = await GetStreetEntity(id);
                db.Entry(street).State = EntityState.Deleted;
                db.Streets.Remove(street);
                await db.SaveChangesAsync();
            }
        }
        public async Task DeleteBuilding(Guid id) 
        {
            using (MUEContext db = new MUEContext())
            {
                Building building = await GetBuildingEntity(id);
                db.Entry(building).State = EntityState.Deleted;
                db.Buildings.Remove(building);
                await db.SaveChangesAsync();
            }
        }
        public async Task DeleteFlat(Guid id) 
        {
            using (MUEContext db = new MUEContext())
            {
                Flat flat = await GetFlatEntity(id);
                db.Entry(flat).State = EntityState.Deleted;
                db.Flats.Remove(flat);
                await db.SaveChangesAsync();

            }
        }

        public async Task SetFlatOwner(Guid FlatId, Guid? OwnerId) 
        {
            using (MUEContext db = new MUEContext())
            {
                

                Flat flat = await GetFlatEntity(FlatId);
                flat.OwnersId = OwnerId;
                db.Entry(flat).State = EntityState.Modified;
               
                await db.SaveChangesAsync();
            }
        }
        // TODO: priority 3: Update 
        //
    }
}