using Martiscoin.EventBus;

namespace Martiscoin.Features.PoA.Events
{
    /// <summary>
    /// Event that is executed when federation member is kicked.
    /// </summary>
    /// <seealso cref="EventBase" />
    public class FedMemberKicked : EventBase
    {
        public IFederationMember KickedMember { get; }

        public FedMemberKicked(IFederationMember  kickedMember)
        {
            this.KickedMember = kickedMember;
        }
    }
}
