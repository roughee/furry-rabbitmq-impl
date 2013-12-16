using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;

namespace GetBanks
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
            channel.QueueDeclare("GetBanks-RecipList-Tunnel", true, false, false, null);

            var subscription = new Subscription(channel, "Credit-GetBanks-Tunnel", false);

            BasicDeliverEventArgs deliveryArgs;
            bool gotMessage = subscription.Next(250, out deliveryArgs);

            var message = (CreditScoreMessage) deliveryArgs.Body.ToRequestMessage(typeof(CreditScoreMessage));
            subscription.Ack(deliveryArgs);

            var ruleChecker = new RuleChecker();
            var banksList = ruleChecker.GetBankList(message.CreditScore);
        }
    }
}
