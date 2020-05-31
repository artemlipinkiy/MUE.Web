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
    public class TypeOfServiceService
    {
        public async Task CreateTypeOfService(TypeOfServiceDTO dto)
        {
            Guid id = Guid.NewGuid();
            using (MUEContext db = new MUEContext())
            {
                TypeOfService typeOfService = new TypeOfService
                {
                 Name = dto.Name,
                 Description = dto.Description,
                 IsMeter = dto.IsMeter,
                 UnitOfMeasurment = dto.UnitOfMeasurment,
                 TypeOfServiceId = id
                };
                db.TypeOfServices.Add(typeOfService);
                await db.SaveChangesAsync();
            }
        }
        public async Task DeleteTypeOfService(Guid id) 
        {
            using (MUEContext db = new MUEContext())
            {
                TypeOfService typeOfService = await GetTypeOfServiceEntity(id);
                db.Entry(typeOfService).State = EntityState.Deleted;
                db.TypeOfServices.Remove(typeOfService);
                await db.SaveChangesAsync();
            }
        }
        public async Task<TypeOfServiceDTO> GetTypeOfServiceDTO(Guid id)
        {
            using (MUEContext db = new MUEContext())
            {
                return await db.TypeOfServices.Where(t => t.TypeOfServiceId == id).Select(t => new TypeOfServiceDTO
                {
                    Description = t.Description,
                    IsMeter = t.IsMeter,
                    Name = t.Name,
                    TypeOfServiceId = t.TypeOfServiceId,
                    UnitOfMeasurment = t.UnitOfMeasurment
                }).FirstOrDefaultAsync();
            }
        }
        public async Task<TypeOfServiceDTO> GetTypeOfServiceDTO(string NameTypeOfService)
        {
            using (MUEContext db = new MUEContext())
            {
                return await db.TypeOfServices.Where(t => t.Name == NameTypeOfService).Select(t => new TypeOfServiceDTO
                {
                    Description = t.Description,
                    IsMeter = t.IsMeter,
                    Name = t.Name,
                    TypeOfServiceId = t.TypeOfServiceId,
                    UnitOfMeasurment = t.UnitOfMeasurment
                }).FirstOrDefaultAsync();
            }
        }
        private async Task<TypeOfService> GetTypeOfServiceEntity(Guid id)
        {
            using (MUEContext db = new MUEContext())
            {
                return await db.TypeOfServices.Where(t => t.TypeOfServiceId == id).FirstOrDefaultAsync();
            }
        }
        public async Task<IList<TypeOfServiceDTO>> GetTypeOfServices(bool isMeter = false)
        {
            using (MUEContext db = new MUEContext())
            {
                if (isMeter)
                {
                    return await db.TypeOfServices.Where(t => t.IsMeter == true).Select(t => new TypeOfServiceDTO
                    {
                        Description = t.Description,
                        IsMeter = t.IsMeter,
                        Name = t.Name,
                        TypeOfServiceId = t.TypeOfServiceId,
                        UnitOfMeasurment = t.UnitOfMeasurment
                    }).ToListAsync();
                }
                return await db.TypeOfServices.Select(t => new TypeOfServiceDTO
                {
                    Description = t.Description,
                    IsMeter = t.IsMeter,
                    Name = t.Name,
                    TypeOfServiceId = t.TypeOfServiceId,
                    UnitOfMeasurment = t.UnitOfMeasurment
                }).ToListAsync();
            }
        }

        public async Task<IList<TypeOfServiceDTO>> GetMyTypeOfServices(Guid flatId)
        {
            using (MUEContext db = new MUEContext())
            {
               
                return await db.TypeOfServices.Select(t => new TypeOfServiceDTO
                {
                    Description = t.Description,
                    IsMeter = t.IsMeter,
                    Name = t.Name,
                    TypeOfServiceId = t.TypeOfServiceId,
                    UnitOfMeasurment = t.UnitOfMeasurment,
                }).ToListAsync();
            }
        }
        public async Task<IList<TypeOfServiceDTO>> GetTypeOfServices(string TypeGet, Guid id)
        {
            using (MUEContext db = new MUEContext())
            {
                switch (TypeGet)
                {
                    case "ForFlat":
                        return null;
                    default:
                        return await db.TypeOfServices.Select(t => new TypeOfServiceDTO
                        {
                            Description = t.Description,
                            IsMeter = t.IsMeter,
                            Name = t.Name,
                            TypeOfServiceId = t.TypeOfServiceId,
                            UnitOfMeasurment = t.UnitOfMeasurment
                        }).ToListAsync();
                }
               
            }
        }
    }
}