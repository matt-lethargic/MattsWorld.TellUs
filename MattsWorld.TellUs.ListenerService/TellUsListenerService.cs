using MattsWorld.TellUs.Events.Journeys.Energy;
using MattsWorld.TellUs.ListenerService.Handlers;

namespace MattsWorld.TellUs.ListenerService
{
    internal class TellUsListenerService
    {
        private string _connectionString = "Endpoint=sb://mattsworld.servicebus.windows.net/;SharedAccessKeyName=service-listener;SharedAccessKey=dJEhEnq4zTP9hjSlJD5HFvq8s/8kPlOPs5gPLZjQVG4=";
        private string _topicName = "tellus";
        private string _subscriptionName = "service-listener";
        private TellUsListener _listener;

        public TellUsListenerService()
        {
            
        }

        public void Start()
        {
            _listener = new TellUsListener(_connectionString, _topicName, _subscriptionName);

            var handler = new EnergyCustomerHandler();
            _listener.RegisterHandler<EnergyCustomerCreated>(handler);
        }

        public void Stop()
        {
            _listener = null;
        }        
    }
}