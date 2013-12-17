using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;
using RabbitMQ.Client.Events;
using Common;
using GetCreditScore.CreditBureau;

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

            var channel2 = connection.CreateModel();
            channel2.QueueDeclare("LoanRequestsWithCreditScore", true, false, false, null);

            var consumer = new EventingBasicConsumer();
            consumer.Model = channel;
            consumer.Received += (s, e) =>
                {
                    var message = e.Body.ToRequestMessage<LoanRequestMessage>();

                    Console.WriteLine("received message with SSN " + message.Ssn);
                    var creditScoreService = new CreditScoreServiceClient();
                    var creditScore = creditScoreService.creditScore(message.Ssn.ToString());
                    Console.WriteLine("Credit Score: " + creditScore);
                    channel2.BasicPublish("", "LoanRequestsWithCreditScore", null, new CreditScoreMessage
                    {
                        CreditScore = creditScore,
                        Amount = message.Amount,
                        Duration = message.Duration,
                        Ssn = message.Duration
                    }.ToByteArray());
                };
            channel.BasicConsume("LoanRequests", true, consumer);
        }
    }
}