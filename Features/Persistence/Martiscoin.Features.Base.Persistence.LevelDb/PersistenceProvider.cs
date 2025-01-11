using Martiscoin.Base;
using Martiscoin.Consensus.Chain;
using Martiscoin.Persistence;
using Martiscoin.Persistence.LevelDb;
using Martiscoin.Utilities.Store;
using Microsoft.Extensions.DependencyInjection;

namespace Martiscoin.Features.Base.Persistence.LevelDb
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