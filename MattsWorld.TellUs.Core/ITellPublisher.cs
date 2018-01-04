using System.Collections.Generic;
using System.Threading.Tasks;

namespace MattsWorld.TellUs.Core
{
    public interface ITellPublisher
    {
        Task SendAsync(ITellUsEvent @event, string correlationId = null);
        Task SendAsync(IEnumerable<ITellUsEvent> events, string correlationId = null);
    }
}
