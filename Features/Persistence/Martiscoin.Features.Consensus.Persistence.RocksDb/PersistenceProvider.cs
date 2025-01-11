using Martiscoin.Features.Consensus.CoinViews.Coindb;
using Martiscoin.Interfaces;
using Martiscoin.Persistence;
using Martiscoin.Persistence.RocksDb;
using Microsoft.Extensions.DependencyInjection;

namespace Martiscoin.Features.Consensus.Persistence.RocksDb
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