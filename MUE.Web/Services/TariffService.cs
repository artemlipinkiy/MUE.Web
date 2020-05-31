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
    public class TariffService
    {
        private readonly TypeOfServiceService typeOfServiceService = new TypeOfServiceService();
        private readonly BuildingService buildingService = new BuildingService();
        public async Task CreateTariff(CreateTariffDTO dto) 
        {
            Guid id = Guid.NewGuid();
            var typeOfServiceId = (await typeOfServiceService.GetTypeOfServiceDTO(dto.NameService)).TypeOfServiceId;
            using (MUEContext db = new MUEContext())
            {
                Tariff tariff = new Tariff 
                {
                TariffId = id,
                Value = dto.Value,
                TypeOfServiceId = typeOfServiceId
                };
            }
        }
        public async Task CreateTariff(TariffDTO dto) 
        {
            Guid id = Guid.NewGuid();
            using (MUEContext db = new MUEContext())
            {
                Tariff tariff = new Tariff 
                { 
                Value = dto.Value,
                TypeOfServiceId = dto.TypeOfServiceId,
                TariffId = id,
                FlatId = dto.FlatId
                };
                db.Tariffs.Add(tariff);
                await db.SaveChangesAsync();
            }
        }
        public async Task<TariffDTO> GetTariffDTO(Guid id) 
        {
            using (MUEContext db = new MUEContext())
            {
                var tariff = await db.Tariffs.Where(t => t.TariffId == id).FirstOrDefaultAsync();
                var typeofservice = (await typeOfServiceService.GetTypeOfServiceDTO(tariff.TypeOfServiceId)).Name;
                return await db.Tariffs.Where(t => t.TariffId == id).Select(t => new TariffDTO {
                TariffId = t.TariffId,
                TypeOfServiceId = t.TypeOfServiceId,
                Value = t.Value,
                TypeOfService = typeofservice
                }).FirstOrDefaultAsync();
            }
        }
        public async Task<TariffDTO> GetTariffDTO(Guid flatId, Guid serviceId)
        {

            using (MUEContext db = new MUEContext())
            {
                var tariff = await db.Tariffs.Where(t => t.FlatId == flatId && t.TypeOfServiceId == serviceId).FirstOrDefaultAsync();
                if (tariff == null)
                {
                    return null;
                }
                var typeofservice = (await typeOfServiceService.GetTypeOfServiceDTO(tariff.TypeOfServiceId)).Name;
                return new TariffDTO 
                {
                FlatId = flatId,
                TypeOfServiceId = serviceId,
                TypeOfService = typeofservice,
                TariffId = tariff.TariffId,
                Value = tariff.Value
                };
            }
        }
        private async Task<Tariff> GetTariffEntity(Guid id)
        {
            using (MUEContext db = new MUEContext())
            {
                return await db.Tariffs.Where(t => t.TariffId == id).FirstOrDefaultAsync();
            }
        }
        public async Task DeleteTariff(Guid id) 
        {
            using (MUEContext db = new MUEContext())
            {
                Tariff tariff = await GetTariffEntity(id);
                db.Entry(tariff).State = EntityState.Deleted;
                db.Tariffs.Remove(tariff);
                await db.SaveChangesAsync();
            }
        }
        public async Task<IList<TariffDTO>> GetTariffs() 
        {
            using (MUEContext db =new MUEContext())
            {
                var tariffs = await db.Tariffs.ToListAsync();
                var typeOfServices = await typeOfServiceService.GetTypeOfServices();
                var flat = await buildingService.GetFlats();
                return tariffs.Select(t => new TariffDTO
                {
                    FlatId = t.FlatId,
                    Flat = flat.Where(f => f.FlatId == t.FlatId).Select( f => f.Address).FirstOrDefault(),
                    IsMeter = typeOfServices.Where(tos => tos.TypeOfServiceId == t.TypeOfServiceId).Select(tos => tos.IsMeter).FirstOrDefault(),
                    TariffId = t.TariffId,
                    Value = t.Value,
                    UnitOfMeasurment = typeOfServices.Where(typeofservice => typeofservice.TypeOfServiceId == t.TypeOfServiceId).Select(typeofservice => typeofservice.UnitOfMeasurment).FirstOrDefault(),
                    TypeOfServiceId = t.TypeOfServiceId,
                    TypeOfService = typeOfServices.Where(typeofservice => typeofservice.TypeOfServiceId == t.TypeOfServiceId).FirstOrDefault().Name
                }).ToList();
            }
        }
        public async Task<IList<TariffDTO>> GetMyTariffs(Guid FlatId) 
        {
            using (MUEContext db = new MUEContext())
            {
                var typeOfServices = await typeOfServiceService.GetTypeOfServices();
                var tariffs = await db.Tariffs.Where(t => t.FlatId == FlatId).ToListAsync();
                var flat = await buildingService.GetFlats();
                return tariffs.Select(t => new TariffDTO
                {
                    FlatId = FlatId,
                    TariffId = t.TariffId,
                    Flat = flat.Where(f => f.FlatId == t.FlatId).Select(f => f.Address).FirstOrDefault(),
                    Value = t.Value,
                    TypeOfServiceId = t.TypeOfServiceId,
                    TypeOfService = typeOfServices.Where(typeofservice => typeofservice.TypeOfServiceId == t.TypeOfServiceId).FirstOrDefault().Name,
                    UnitOfMeasurment = typeOfServices.Where(typeofservice => typeofservice.TypeOfServiceId == t.TypeOfServiceId).Select(typeofservice => typeofservice.UnitOfMeasurment).FirstOrDefault(),
                    IsMeter = typeOfServices.Where(typeofservice => typeofservice.TypeOfServiceId == t.TypeOfServiceId).Select(typeofservice => typeofservice.IsMeter).FirstOrDefault()
                }).ToList();
            }
        }
        public async Task<IList<TariffDTO>> GetAllMyTariffs(Guid ownerId)
        {
            using (MUEContext db = new MUEContext())
            {
                var typeOfServices = await typeOfServiceService.GetTypeOfServices();
                //var tariffs = await db.Tariffs.Where(t => t.FlatId == FlatId).ToListAsync();
                var myflatIds = await db.Flats.Where(f => f.OwnersId == ownerId).Select(f =>f.FlatId).ToListAsync();
                var tariffs = await db.Tariffs.Where(t => myflatIds.Contains(t.FlatId)).ToListAsync();
                var flat = await buildingService.GetFlats();
                return tariffs.Select(t => new TariffDTO
                {
                    FlatId = t.FlatId,
                    TariffId = t.TariffId,
                    Value = t.Value,
                    Flat = flat.Where(f => f.FlatId == t.FlatId).Select(f => f.Address).FirstOrDefault(),
                    TypeOfServiceId = t.TypeOfServiceId,
                    TypeOfService = typeOfServices.Where(typeofservice => typeofservice.TypeOfServiceId == t.TypeOfServiceId).Select(typeofservice => typeofservice.Name).FirstOrDefault(),
                    UnitOfMeasurment = typeOfServices.Where(typeofservice => typeofservice.TypeOfServiceId == t.TypeOfServiceId).Select(typeofservice => typeofservice.UnitOfMeasurment).FirstOrDefault(),
                    IsMeter = typeOfServices.Where(typeofservice => typeofservice.TypeOfServiceId == t.TypeOfServiceId).Select(typeofservice => typeofservice.IsMeter).FirstOrDefault()
                }).ToList();
            }
        }

    }
}