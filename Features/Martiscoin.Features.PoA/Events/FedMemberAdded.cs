using Martiscoin.EventBus;

namespace Martiscoin.Features.PoA.Events
{
    /// <summary>
    /// Event that is executed when a new federation member is added.
    /// </summary>
    /// <seealso cref="EventBase" />
    public class FedMemberAdded : EventBase
    {
        public IFederationMember AddedMember { get; }

        public FedMemberAdded(IFederationMember addedMember)
        {
            this.AddedMember = addedMember;
        }
    }
}
