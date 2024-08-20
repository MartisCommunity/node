using System;
using XOuranos.Broadcasters;
using XOuranos.EventBus;
using XOuranos.Features.Miner.Api.Models;

namespace XOuranos.Features.Miner.Broadcasters
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