using Martiscoin.Base;
using Martiscoin.Consensus.Chain;
using Martiscoin.Persistence;
using Martiscoin.Persistence.RocksDb;
using Martiscoin.Utilities.Store;
using Microsoft.Extensions.DependencyInjection;

namespace Martiscoin.Features.Base.Persistence.RocksDb
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