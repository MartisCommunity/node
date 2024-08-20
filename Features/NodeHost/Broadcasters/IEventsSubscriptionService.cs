using System.Threading.Tasks;
using XOuranos.Broadcasters;
using XOuranos.EventBus;
using Microsoft.AspNetCore.SignalR;

namespace XOuranos.Broadcasters
{
    public interface IEventsSubscriptionService
    {
        void Init();

        bool HasConsumers { get; }

        void Subscribe(string id, string name);

        void Unsubscribe(string id, string name);

        void UnsubscribeAll(string id);

        void OnEvent(EventBase @event);

        void SetHub<T>(IHubContext<T> hubContext) where T : Hub;
    }
}