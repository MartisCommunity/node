using System.Collections.Generic;
using Martiscoin.AsyncWork;
using Martiscoin.Broadcasters;
using Martiscoin.EventBus;
using Martiscoin.Features.Miner.Interfaces;
using Martiscoin.Utilities;
using Microsoft.Extensions.Logging;

namespace Martiscoin.Features.Miner.Broadcasters
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