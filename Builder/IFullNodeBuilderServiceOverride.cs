using System;
using XOuranos.Builder.Feature;
using XOuranos.Configuration;
using XOuranos.Networks;
using XOuranos.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace XOuranos.Builder
{
    /// <summary>
    /// Allow specific network implementation to override services.
    /// </summary>
    public interface IFullNodeBuilderServiceOverride
    {
        /// <summary>
        /// Intercept the builder to override services.
        /// </summary>
        void OverrideServices(IFullNodeBuilder builder);
    }
}
