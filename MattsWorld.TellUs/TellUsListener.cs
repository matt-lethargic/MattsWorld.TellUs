using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MattsWorld.TellUs.Core;
using MattsWorld.TellUs.Events;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace MattsWorld.TellUs
{
    public class TellUsListener : ITellUsListener
    {
        private readonly ISubscriptionClient _subscriptionClient;
        private Func<ExceptionReceivedEventArgs, Task> _exceptionHandler;


        private readonly Dictionary<Type, ITellUsEventHandler> _handlers;

        public TellUsListener(string connectionString, string topicName, string subscriptionName)
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 10,
                AutoComplete = false
            };

            _subscriptionClient = new SubscriptionClient(connectionString, topicName, subscriptionName);
            _subscriptionClient.RegisterMessageHandler(ProcessMessageAsync, messageHandlerOptions);

            _handlers = new Dictionary<Type, ITellUsEventHandler>();
        }


        public void RegisterHandler<TEvent>(ITellUsEventHandler handler) where TEvent : ITellUsEvent
        {
            Type eventType = typeof(TEvent);
            _handlers.Add(eventType, handler);
        }
        
        public void RegisterExceptionHandler(Func<ExceptionReceivedEventArgs, Task> exceptionHandler)
        {
            _exceptionHandler = exceptionHandler;
        }
        

        private async Task ProcessMessageAsync(Message message, CancellationToken cancellationToken)
        {
            if (_handlers.Count == 0)
                return;

            string eventType = message.UserProperties[Constants.EventTypeProperty].ToString();
            string messageBody = Encoding.UTF8.GetString(message.Body);

            System.Type messageBodyType = Type.GetType(eventType);
            object eventObject = JsonConvert.DeserializeObject(messageBody, messageBodyType);

            if (eventObject is TellUsEvent tellUsEvent)
            {
                if (_handlers.ContainsKey(messageBodyType))
                {
                    await _handlers[messageBodyType].Handle(tellUsEvent, message.CorrelationId, cancellationToken);
                    await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
                }
            }
            else
            {
                await _subscriptionClient.DeadLetterAsync(message.SystemProperties.LockToken);
            }
        }

        private async Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            if(_exceptionHandler == null)
                return;

            await _exceptionHandler(exceptionReceivedEventArgs);
        }
    }
}