using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;
using Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace LoanRequestCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
            };

            var connection = factory.CreateConnection();

            var channel = connection.CreateModel();
            channel.QueueDeclare("LoanRequests", true, false, false, null);
                
            var subscription = new Subscription(channel, "LoanRequests", false);

            for (int i = 1; i < 10; i++)
            {
                var message = new LoanRequestMessage { Amount = i * 10000, Duration = i * 10, Ssn = 12312312 };
                channel.BasicPublish("", "LoanRequests", null, message.ToByteArray()); 
            }

            Environment.Exit(0);
        }
    }
}
