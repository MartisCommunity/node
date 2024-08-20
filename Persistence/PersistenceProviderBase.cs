using System;
using XOuranos.Builder.Feature;
using Microsoft.Extensions.DependencyInjection;

namespace XOuranos.Persistence
{
    /// <inheritdoc/>
    public abstract class PersistenceProviderBase<TFeature> : IPersistenceProvider where TFeature : IFullNodeFeature
    {
        /// <inheritdoc/>
        public abstract string Tag { get; }

        /// <inheritdoc/>
        public Type FeatureType => typeof(TFeature);

        /// <inheritdoc/>
        public abstract void AddRequiredServices(IServiceCollection services);
    }
}
