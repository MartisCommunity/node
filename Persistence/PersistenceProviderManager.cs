using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using XOuranos.Builder;
using XOuranos.Builder.Feature;
using XOuranos.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace XOuranos.Persistence
{
    /// <inheritdoc/>
    public class PersistenceProviderManager : IPersistenceProviderManager
    {
        protected readonly NodeSettings nodeSettings;
        protected readonly Dictionary<string, List<IPersistenceProvider>> persistenceProviders;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistenceProviderManager"/> class.
        /// This class handles the initialization of persistence implementors for specific features on behalf of other features.
        /// For example if BlockStore feature requires a persistence, it can call
        /// </summary>
        /// <param name="nodeSettings">The settings from which obtain the default db type.</param>
        public PersistenceProviderManager(NodeSettings nodeSettings)
        {
            this.nodeSettings = nodeSettings;
            this.persistenceProviders = new Dictionary<string, List<IPersistenceProvider>>();
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetAvailableProviders()
        {
            return this.persistenceProviders.Keys;
        }

        /// <inheritdoc/>
        public string GetDefaultProvider()
        {
            if (this.persistenceProviders.Count == 0)
            {
                return null;
            }

            return this.persistenceProviders.ContainsKey("leveldb") ? "leveldb" : this.persistenceProviders.Keys.First();
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Search for all assemblies that implement the interface IPersistenceProvider in all referenced libraries.
        /// Create one instance for each of them and register the instance in the <see cref="persistenceProviders"/> dictionary in order to be found when <see cref="RequirePersistence"/> will be called by any feature
        /// </remarks>
        public virtual void Initialize()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type providerType = assembly.GetType("XOuranos.Features.Base.Persistence.LevelDb.PersistenceProvider");
            IPersistenceProvider providerInstance = (IPersistenceProvider)Activator.CreateInstance(providerType);
            var persistenceproviders = new List<IPersistenceProvider>();
            persistenceproviders.Add(providerInstance);

            providerType = assembly.GetType("XOuranos.Features.BlockStore.Persistence.LevelDb.PersistenceProvider");
            IPersistenceProvider providerInstance2 = (IPersistenceProvider)Activator.CreateInstance(providerType);
            persistenceproviders.Add(providerInstance2);

            providerType = assembly.GetType("XOuranos.Features.Consensus.Persistence.LevelDb.PowPersistenceProvider");
            IPersistenceProvider providerInstance3 = (IPersistenceProvider)Activator.CreateInstance(providerType);
            persistenceproviders.Add(providerInstance3);

            providerType = assembly.GetType("XOuranos.Features.Consensus.Persistence.LevelDb.PosPersistenceProvider");
            IPersistenceProvider providerInstance4 = (IPersistenceProvider)Activator.CreateInstance(providerType);
            persistenceproviders.Add(providerInstance4);

            this.persistenceProviders.Add("leveldb", persistenceproviders);
        }


        /// <inheritdoc/>
        public void RequirePersistence<TFeature>(IServiceCollection services, string persistenceProviderImplementation = null) where TFeature : IFullNodeFeature
        {
            if (persistenceProviderImplementation == null)
            {
                persistenceProviderImplementation = this.nodeSettings.DbType ?? this.GetDefaultProvider();
            }

            IPersistenceProvider provider = null;

            if (this.persistenceProviders.TryGetValue(persistenceProviderImplementation.ToLowerInvariant(), out List<IPersistenceProvider> providersList))
            {
                provider = providersList.FirstOrDefault(provider => provider.FeatureType == typeof(TFeature));

                if (provider == null)
                {
                    throw new NodeBuilderException($"Required persistence provider {persistenceProviderImplementation} doesn't implement persistence for {typeof(TFeature).Name}.");
                }

                provider.AddRequiredServices(services);
            }
            else
            {
                throw new NodeBuilderException($"Required persistence provider implementation {persistenceProviderImplementation} Not found.");
            }
        }
    }
}