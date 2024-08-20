using XOuranos.Features.BlockStore.Pruning;
using XOuranos.Features.BlockStore.Repository;
using XOuranos.Persistence;
using XOuranos.Persistence.RocksDb;
using Microsoft.Extensions.DependencyInjection;

namespace XOuranos.Features.BlockStore.Persistence.RocksDb
{
    public class PersistenceProvider : PersistenceProviderBase<BlockStoreFeature>
    {
        public override string Tag => RocksDbPersistence.Name;

        public override void AddRequiredServices(IServiceCollection services)
        {
            services.AddSingleton<IBlockRepository, RocksdbBlockRepository>();
            services.AddSingleton<IPrunedBlockRepository, RocksDbPrunedBlockRepository>();
        }
    }
}