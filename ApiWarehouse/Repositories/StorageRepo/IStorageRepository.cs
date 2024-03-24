using WareHouse.DTO.StorageDto;

namespace WareHouse.Repositories.StorageRepo
{
    public interface IStorageRepository
    {
        Task<IEnumerable<GetStorageDto>> GetAllStoragesAsync();
        Task<Guid?> AddStorageAsync(StorageDto storageDto);
        Task<Guid?> DeleteStorageAsync(Guid storageId);
        Task<Guid?> UpdateStorageAsync(UpdateStorageDto updateStorageDto);
    }
}
