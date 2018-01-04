using System;
using System.Threading.Tasks;
using MattsWorld.TellUs.Events.Journeys.Energy;

namespace MattsWorld.TellUs.MessageWriter
{
    class Program
    {
        private static string _connectionString = "Endpoint=sb://mattsworld.servicebus.windows.net/;SharedAccessKeyName=service-writer;SharedAccessKey=WZpsd/HT6JDnASQU1ypKQ5BTKpzT6r25WzUUJJWqnYc=";
        private static string _topicName = "tellus";

        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            TellUsPublisher publisher = new TellUsPublisher(_connectionString, _topicName);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Message:");
                var message = Console.ReadLine();

                var evet = new EnergyCustomerCreated
                {
                    Message = message
                };
                await publisher.SendAsync(evet);

                Console.Clear();
                Console.WriteLine("Message sent, send another?");
                var keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.Escape || keyInfo.Key == ConsoleKey.Q)
                {
                    break;
                }
            }
        }
    }
}
