using Microsoft.Azure.ServiceBus;
using SBShared.Models;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SBReceiver
{
    class Program
    {
        const string connectionString = "";
        const string queueName = "personqueue";
        static IQueueClient queueClient;

        static async Task Main(string[] args)
        {
            queueClient = new QueueClient(connectionString, queueName);
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };
            queueClient.RegisterMessageHandler(ProcessMessage, messageHandlerOptions);
            Console.ReadLine();
            await queueClient.CloseAsync();
        }

        private static async Task ProcessMessage(Message message, CancellationToken token)
        {
            var jsonstring = Encoding.UTF8.GetString(message.Body);
            PersonModel person = JsonSerializer.Deserialize<PersonModel>(jsonstring);
            Console.WriteLine($"Person Received: { person.FirstName } { person.LastName }");
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            Console.WriteLine($"Message handler exception: { arg.Exception.Message }");
            return Task.CompletedTask;
        }
    }
}
