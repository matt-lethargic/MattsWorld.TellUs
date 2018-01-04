using System.Threading;
using System.Threading.Tasks;

namespace MattsWorld.TellUs.Core
{
    public interface ITellUsEventHandler
    {
        Task Handle(ITellUsEvent @event, string correlationId, CancellationToken token);
    }
}
