using System.Collections.Generic;
using System.Linq;
using Martiscoin.Base;
using Martiscoin.Consensus;
using Martiscoin.Consensus.Chain;
using Martiscoin.Features.BlockStore;
using Martiscoin.Features.PoA.Payloads;
using Martiscoin.Interfaces;
using Martiscoin.P2P.Protocol.Payloads;
using Microsoft.Extensions.Logging;

namespace Martiscoin.Features.PoA.Behaviors
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