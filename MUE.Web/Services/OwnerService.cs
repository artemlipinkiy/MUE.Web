using MUE.Web.Entities;
using MUE.Web.EntitiesDTO.UserDTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MUE.Web.Services
{
    public class OwnerService
    {
        public async Task Update(OwnerDTO dto) 
        {
            var owner = await GetEntity(dto.OwnerId);
            using (MUEContext db = new MUEContext())
            {
                owner.FirstName = dto.FirstName;
                owner.LastName = dto.LastName;
                owner.MiddlleName = dto.MiddlleName;
                owner.Status = dto.Status;
                db.Entry(owner).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
        }
        public async Task<Guid> Create(OwnerDTO dto) 
        {
            Guid id = Guid.NewGuid();
            using (MUEContext db =new MUEContext())
            {
                Owner owner = new Owner
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    MiddlleName = dto.MiddlleName,
                    OwnerId = id,
                    Status = dto.Status
                };
                db.Owners.Add(owner);
                await db.SaveChangesAsync();
                return id;
            }
        }
        public async Task<OwnerDTO> GetDTO(Guid id) 
        {
            using (MUEContext db = new MUEContext())
            {
                return await db.Owners.Where(o => o.OwnerId == id).Select(o => new OwnerDTO {
                OwnerId = o.OwnerId,
                FirstName = o.FirstName,
                LastName = o.LastName,
                MiddlleName = o.MiddlleName,
                Status = o.Status
                }).FirstOrDefaultAsync();
            }
        }
        public async Task<IList<OwnerDTO>> GetOwners() 
        {
            using (MUEContext db = new MUEContext())
            {
                return await db.Owners.Select(o => new OwnerDTO { 
                    FirstName = o.FirstName,
                    LastName = o.LastName,
                    MiddlleName = o.MiddlleName,
                    OwnerId = o.OwnerId,
                    Status = o.Status
                    
                    }).ToListAsync();
            }
        }
        public async Task Delete(Guid id) 
        {
            using (MUEContext db = new MUEContext())
            {
                Owner owner = await GetEntity(id);
                db.Entry(owner).State = EntityState.Deleted;
                db.Owners.Remove(owner);
                await db.SaveChangesAsync();
            }
        }
        private async Task<Owner> GetEntity(Guid id) 
        {
            using (MUEContext db = new MUEContext())
            {
                return await db.Owners.Where(o => o.OwnerId == id).FirstOrDefaultAsync();
            }
        }

    }
}