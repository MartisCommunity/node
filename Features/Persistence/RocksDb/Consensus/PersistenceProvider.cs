using XOuranos.Features.Consensus.CoinViews.Coindb;
using XOuranos.Interfaces;
using XOuranos.Persistence;
using XOuranos.Persistence.RocksDb;
using Microsoft.Extensions.DependencyInjection;

namespace XOuranos.Features.Consensus.Persistence.RocksDb
{
    public class PowPersistenceProvider : PersistenceProviderBase<PowConsensusFeature>
    {
        public override string Tag => RocksDbPersistence.Name;

        public override void AddRequiredServices(IServiceCollection services)
        {
            services.AddSingleton<ICoindb, RocksDbCoindb>();
        }
    }

    public class PosPersistenceProvider : PersistenceProviderBase<PosConsensusFeature>
    {
        public override string Tag => RocksDbPersistence.Name;

        public override void AddRequiredServices(IServiceCollection services)
        {
            services.AddSingleton<ICoindb, RocksDbCoindb>();
            services.AddSingleton<IProvenBlockHeaderRepository, RocksDbProvenBlockHeaderRepository>();
        }
    }
}