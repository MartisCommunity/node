using XOuranos.Base;
using XOuranos.Consensus.Chain;
using XOuranos.Persistence;
using XOuranos.Persistence.LevelDb;
using XOuranos.Utilities.Store;
using Microsoft.Extensions.DependencyInjection;

namespace XOuranos.Features.Base.Persistence.LevelDb
{
    public class PersistenceProvider : PersistenceProviderBase<BaseFeature>
    {
        public override string Tag => LevelDbPersistence.Name;

        public override void AddRequiredServices(IServiceCollection services)
        {
            services.AddSingleton<IChainStore, LevelDbChainStore>();
            services.AddSingleton<IKeyValueRepository, LevelDbKeyValueRepository>();
        }
    }
}