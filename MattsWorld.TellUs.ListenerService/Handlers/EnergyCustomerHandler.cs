using System;
using System.Threading;
using System.Threading.Tasks;
using MattsWorld.TellUs.Core;
using MattsWorld.TellUs.Events.Journeys.Energy;

namespace MattsWorld.TellUs.ListenerService.Handlers
{
    public class EnergyCustomerHandler : ITellUsEventHandler
    {
        public Task Handle(ITellUsEvent @event, string correlationId, CancellationToken token)
        {
            if (!(@event is EnergyCustomerCreated createdEvent))
                return null;
            
            Console.WriteLine(createdEvent.Message);

            return Task.CompletedTask;
        }
    }
}
