using System.Collections.Generic;
using System.Linq;
using XOuranos.Base;
using XOuranos.Consensus;
using XOuranos.Consensus.Chain;
using XOuranos.Features.BlockStore;
using XOuranos.Features.PoA.Payloads;
using XOuranos.Interfaces;
using XOuranos.P2P.Protocol.Payloads;
using Microsoft.Extensions.Logging;

namespace XOuranos.Features.PoA.Behaviors
{
    public class PoABlockStoreBehavior : BlockStoreBehavior
    {
        public PoABlockStoreBehavior(ChainIndexer chainIndexer, IChainState chainState, ILoggerFactory loggerFactory, IConsensusManager consensusManager, IBlockStoreQueue blockStoreQueue)
            : base(chainIndexer, chainState, loggerFactory, consensusManager, blockStoreQueue)
        {
        }

        /// <inheritdoc />
        protected override Payload BuildHeadersAnnouncePayload(IEnumerable<ChainedHeader> headers)
        {
            var poaHeaders = headers.Select(s => s.Header).Cast<PoABlockHeader>().ToList();

            return new PoAHeadersPayload(poaHeaders);
        }

        public override object Clone()
        {
            var res = new PoABlockStoreBehavior(this.ChainIndexer, this.chainState, this.loggerFactory, this.consensusManager, this.blockStoreQueue)
            {
                CanRespondToGetBlocksPayload = this.CanRespondToGetBlocksPayload,
                CanRespondToGetDataPayload = this.CanRespondToGetDataPayload
            };

            return res;
        }
    }
}