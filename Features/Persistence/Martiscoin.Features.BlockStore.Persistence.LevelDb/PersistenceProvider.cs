using Martiscoin.Features.BlockStore.Pruning;
using Martiscoin.Features.BlockStore.Repository;
using Martiscoin.Persistence;
using Martiscoin.Persistence.LevelDb;
using Microsoft.Extensions.DependencyInjection;

namespace Martiscoin.Features.BlockStore.Persistence.LevelDb
{
    public class PersistenceProvider : PersistenceProviderBase<BlockStoreFeature>
    {
        public override string Tag => LevelDbPersistence.Name;

        public override void AddRequiredServices(IServiceCollection services)
        {
            services.AddSingleton<IBlockRepository, LevelDbBlockRepository>();
            services.AddSingleton<IPrunedBlockRepository, LevelDbPrunedBlockRepository>();
        }
    }
}