using System.Collections.Generic;
using XOuranos.AsyncWork;
using XOuranos.Broadcasters;
using XOuranos.EventBus;
using XOuranos.Features.Miner.Interfaces;
using XOuranos.Utilities;
using Microsoft.Extensions.Logging;

namespace XOuranos.Features.Miner.Broadcasters
{
    /// <summary>
    /// Broadcasts current staking information to Web Socket clients
    /// </summary>
    public class StakingBroadcaster : ClientBroadcasterBase
    {
        private readonly IPosMinting posMinting;

        public StakingBroadcaster(
            ILoggerFactory loggerFactory,
            IPosMinting posMinting,
            INodeLifetime nodeLifetime,
            IAsyncProvider asyncProvider,
            IEventsSubscriptionService subscriptionService = null)
            : base(loggerFactory, nodeLifetime, asyncProvider, subscriptionService)
        {
            this.posMinting = posMinting;
        }

        protected override IEnumerable<EventBase> GetMessages()
        {
            if (null != this.posMinting)
            {
                yield return new StakingInfoClientEvent(this.posMinting.GetGetStakingInfoModel());
            }
        }
    }
}