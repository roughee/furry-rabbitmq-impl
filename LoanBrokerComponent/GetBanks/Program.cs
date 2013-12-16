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
            channel.QueueDeclare("LoanRequestsWithCreditScore", true, false, false, null);

            var subscription = new Subscription(channel, "LoanRequestsWithCreditScore", false);

            var channel2 = connection.CreateModel();
            channel2.QueueDeclare("BanksList", true, false, false, null);

            var consumer = new EventingBasicConsumer();
            consumer.Model = channel;
            consumer.Received += (s, e) =>
            {
                var message = e.Body.ToRequestMessage<CreditScoreMessage>();

                var ruleChecker = new RuleChecker();
                var banksList = ruleChecker.GetBankList(message.CreditScore);

                channel2.BasicPublish("", "BanksList", null, new BanksListMessage
                {
                    BanksList = banksList,
                    CreditScore = message.CreditScore,
                    Amount = message.Amount,
                    Duration = message.Duration,
                    Ssn = message.Duration
                }.ToByteArray());
            };
            channel.BasicConsume("LoanRequestsWithCreditScore", true, consumer);
        }
    }
}
