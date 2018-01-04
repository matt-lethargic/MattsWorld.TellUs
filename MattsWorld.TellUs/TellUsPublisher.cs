using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MattsWorld.TellUs.Core;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace MattsWorld.TellUs
{
    public class TellUsPublisher : ITellPublisher
    {
        private readonly ITopicClient _topicClient;

        public TellUsPublisher(string connectionString, string topicName)
        {
            _topicClient = new TopicClient(connectionString, topicName);
        }

        public async Task SendAsync(ITellUsEvent @event, string correlationId = null)
        {
            Message message = CreateMessage(@event, correlationId);
            await _topicClient.SendAsync(message);
        }

        public async Task SendAsync(IEnumerable<ITellUsEvent> events, string correlationId = null)
        {
            IList<Message> messages = events.Select(x => CreateMessage(x, correlationId)).ToList();
            await _topicClient.SendAsync(messages);
        }

        private Message CreateMessage(ITellUsEvent @event, string correlationId)
        {
            string messageBody = JsonConvert.SerializeObject(@event);
            Message message = new Message(Encoding.UTF8.GetBytes(messageBody));
            message.UserProperties.Add(Constants.EventTypeProperty, @event.Type);
            message.UserProperties.Add(Constants.EventNameProperty, @event.Name);
            message.UserProperties.Add(Constants.EventCategoryProperty, @event.Category);
            message.CorrelationId = correlationId;
            message.ContentType = "application/json";
            return message;
        }
    }
}