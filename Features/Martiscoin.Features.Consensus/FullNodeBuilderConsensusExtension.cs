using Martiscoin.Base;
using Martiscoin.Builder;
using Martiscoin.Configuration.Logging;
using Martiscoin.Consensus;
using Martiscoin.Features.Consensus.CoinViews;
using Martiscoin.Features.Consensus.CoinViews.Coindb;
using Martiscoin.Features.Consensus.Interfaces;
using Martiscoin.Features.Consensus.ProvenBlockHeaders;
using Martiscoin.Features.Consensus.Rules;
using Martiscoin.Interfaces;
using Martiscoin.Utilities.Store;
using Microsoft.Extensions.DependencyInjection;

namespace Martiscoin.Features.Consensus
{
    /// <summary>
    /// A class providing extension methods for <see cref="IFullNodeBuilder"/>.
    /// </summary>
    public static class FullNodeBuilderConsensusExtension
    {
        public static IFullNodeBuilder UsePowConsensus(this IFullNodeBuilder fullNodeBuilder)
        {
            LoggingConfiguration.RegisterFeatureNamespace<PowConsensusFeature>("powconsensus");

            fullNodeBuilder.ConfigureFeature(features =>
            {
                features
                    .AddFeature<PowConsensusFeature>()
                    .FeatureServices(services =>
                    {
                        fullNodeBuilder.PersistenceProviderManager.RequirePersistence<PowConsensusFeature>(services);

                        services.AddSingleton<ConsensusOptions, ConsensusOptions>();
                        services.AddSingleton<ICoinView, CachedCoinView>();
                        services.AddSingleton<IConsensusRuleEngine, PowConsensusRuleEngine>();
                        services.AddSingleton<IChainState, ChainState>();
                        services.AddSingleton<ConsensusQuery>()
                            .AddSingleton<INetworkDifficulty, ConsensusQuery>(provider => provider.GetService<ConsensusQuery>())
                            .AddSingleton<IGetUnspentTransaction, ConsensusQuery>(provider => provider.GetService<ConsensusQuery>());
                    });
            });

            return fullNodeBuilder;
        }

        public static IFullNodeBuilder UsePosConsensus(this IFullNodeBuilder fullNodeBuilder)
        {
            LoggingConfiguration.RegisterFeatureNamespace<PosConsensusFeature>("posconsensus");

            fullNodeBuilder.ConfigureFeature(features =>
            {
                features
                    .AddFeature<PosConsensusFeature>()
                    .FeatureServices(services =>
                    {
                        fullNodeBuilder.PersistenceProviderManager.RequirePersistence<PosConsensusFeature>(services);

                        services.AddSingleton<IStakdb>(provider => (IStakdb)provider.GetService<ICoindb>());
                        services.AddSingleton<ICoinView, CachedCoinView>();
                        services.AddSingleton<StakeChainStore>().AddSingleton<IStakeChain, StakeChainStore>(provider => provider.GetService<StakeChainStore>());
                        services.AddSingleton<IStakeValidator, StakeValidator>();
                        services.AddSingleton<IRewindDataIndexCache, RewindDataIndexCache>();
                        services.AddSingleton<IConsensusRuleEngine, PosConsensusRuleEngine>();
                        services.AddSingleton<IChainState, ChainState>();
                        services.AddSingleton<ConsensusQuery>()
                            .AddSingleton<INetworkDifficulty, ConsensusQuery>(provider => provider.GetService<ConsensusQuery>())
                            .AddSingleton<IGetUnspentTransaction, ConsensusQuery>(provider => provider.GetService<ConsensusQuery>());
                        services.AddSingleton<IProvenBlockHeaderStore, ProvenBlockHeaderStore>();
                    });
            });

            return fullNodeBuilder;
        }
    }
}