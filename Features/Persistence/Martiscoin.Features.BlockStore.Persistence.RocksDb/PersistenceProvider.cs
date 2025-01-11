using Martiscoin.Features.BlockStore.Pruning;
using Martiscoin.Features.BlockStore.Repository;
using Martiscoin.Persistence;
using Martiscoin.Persistence.RocksDb;
using Microsoft.Extensions.DependencyInjection;

namespace Martiscoin.Features.BlockStore.Persistence.RocksDb
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