using System;
using Martiscoin.Broadcasters;
using Martiscoin.EventBus;
using Martiscoin.Features.Miner.Api.Models;

namespace Martiscoin.Features.Miner.Broadcasters
{
    public class StakingInfoClientEvent : EventBase
    {
        public StakingInfoClientEvent(GetStakingInfoModel stakingInfoModel)
        {
            this.StakingInfo = stakingInfoModel;
        }

        public GetStakingInfoModel StakingInfo { get; set; }
    }
}