using XOuranos.Base;
using XOuranos.Consensus.Chain;
using XOuranos.Persistence;
using XOuranos.Persistence.RocksDb;
using XOuranos.Utilities.Store;
using Microsoft.Extensions.DependencyInjection;

namespace XOuranos.Features.Base.Persistence.RocksDb
{
    public class PersistenceProvider : PersistenceProviderBase<BaseFeature>
    {
        public override string Tag => RocksDbPersistence.Name;

        public override void AddRequiredServices(IServiceCollection services)
        {
            services.AddSingleton<IChainStore, RocksDbChainStore>();
            services.AddSingleton<IKeyValueRepository, RocksDbKeyValueRepository>();
        }
    }
}