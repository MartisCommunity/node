using System.Threading.Tasks;
using Martiscoin.Broadcasters;
using Martiscoin.EventBus;
using Microsoft.AspNetCore.SignalR;

namespace Martiscoin.Broadcasters
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