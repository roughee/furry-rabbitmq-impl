using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;
using RabbitMQ.Client.Events;
using Common;

namespace GetCreditScore
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

            BasicDeliverEventArgs deliveryArgs;
            bool gotMessage = subscription.Next(250, out deliveryArgs);

            var message = deliveryArgs.Body.ToLoanRequestMessage();
            subscription.Ack(deliveryArgs);
        }
    }
}