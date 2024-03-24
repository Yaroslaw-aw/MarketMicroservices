using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WareHouse.DTO.StorageDto;
using WareHouse.Models;

namespace WareHouse.Repositories.StorageRepo
{
    public class StorageRepository : IStorageRepository
    {
        private readonly WareHouseContext context;
        private readonly IMapper mapper;

        public StorageRepository(WareHouseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<GetStorageDto>> GetAllStoragesAsync()
        {
            List<GetStorageDto> storagesDto = new List<GetStorageDto>();

            try
            {
                using (context)
                {
                    List<Storage>? storages = await context.Storages.AsNoTracking().ToListAsync();
                    storagesDto = mapper.Map(storages, storagesDto);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return storagesDto;
        }

        public async Task<Guid?> AddStorageAsync(StorageDto storageDto)
        {
            using (context)
            {
                Storage? existingStorage = await context.Storages.FirstOrDefaultAsync(es => es.Name == storageDto.Name);

                if (existingStorage != null) return existingStorage.Id;

                Storage newStorage = mapper.Map<Storage>(storageDto);
                context.Storages.Add(newStorage);
                await context.SaveChangesAsync();
                return newStorage.Id;
            }
        }

        public async Task<Guid?> DeleteStorageAsync(Guid deleteStorageId)
        {
            using (context)
            {
                Storage? existingStorage = await context.Storages.FirstOrDefaultAsync(ec => ec.Id == deleteStorageId);
                if (existingStorage is null) return null;
                context.Remove(existingStorage);
                await context.SaveChangesAsync();
                return existingStorage.Id;
            }
        }


        public async Task<Guid?> UpdateStorageAsync(UpdateStorageDto updateStorageDto)
        {
            using (context)
            {
                Storage? existingStorage = await context.Storages.FirstOrDefaultAsync(ec => ec.Id == updateStorageDto.Id);
                if (existingStorage is null) return null;
                mapper.Map(updateStorageDto, existingStorage);
                context.SaveChanges();
                return existingStorage.Id;
            }
        }
    }
}
