using Market.DTO.Caching;
using Microsoft.AspNetCore.Mvc;
using WareHouse.DTO.StorageDto;
using WareHouse.Repositories.StorageRepo;

namespace WareHouse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StorageController : Controller
    {
        private readonly IStorageRepository repository;
        private readonly Redis redis;

        public StorageController(IStorageRepository repository, Redis redis)
        {
            this.repository = repository;
            this.redis = redis;
        }

        [HttpGet(template: "GetAllStorages")]
        public async Task<ActionResult<IEnumerable<GetStorageDto>>> GetAllStorages()
        {
            if (redis.TryGetValue("storages", out List<GetStorageDto>? storagesCache) && storagesCache != null)
                return AcceptedAtAction(nameof(GetAllStorages), storagesCache);

            IEnumerable<GetStorageDto>? storages = await repository.GetAllStoragesAsync();

            redis.SetData("storages", storages);

            return AcceptedAtAction(nameof (GetAllStorages), storages);
        }

        [HttpPost(template: "AddStorage")]
        public async Task<ActionResult<Guid?>> AddStorage(StorageDto storageDto)
        {
            Guid? newStorageId = await repository.AddStorageAsync(storageDto);

            await redis.cache.RemoveAsync("storages");

            return AcceptedAtAction(nameof(AddStorage), newStorageId);
        }

        [HttpDelete(template: "DeleteStorage")]
        public async Task<ActionResult<Guid?>> DeleteStorage(Guid deleteStorageId)
        {
            Guid? deletedStorageId = await repository.DeleteStorageAsync(deleteStorageId);

            if (deletedStorageId == null) return NotFound();

            await redis.cache.RemoveAsync("storages");

            return AcceptedAtAction(nameof(DeleteStorage), deletedStorageId);
        }

        [HttpPut(template: "UpdateStorage")]
        public async Task<ActionResult<Guid?>> UpdateStorage(UpdateStorageDto updateStorageDto)
        {
            Guid? updatedStorageId = await repository.UpdateStorageAsync(updateStorageDto);

            if (updatedStorageId == null) return NotFound();

            await redis.cache.RemoveAsync("storages");

            return AcceptedAtAction(nameof(UpdateStorage), updatedStorageId);
        }
    }
}
