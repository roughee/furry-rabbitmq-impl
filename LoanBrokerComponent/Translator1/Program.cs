using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing.v0_9_1;

namespace Translator1
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            var factory2 = new ConnectionFactory
            {
                HostName = "datdb.cphbusiness.dk",
                UserName = "student",
                Password = "cph",
                Port = 5672
            };

            var connection = factory.CreateConnection();
            var cphConnection = factory2.CreateConnection();

            var channel = connection.CreateModel();
            channel.QueueDeclare("Bank1Requests", true, false, false, null);

            var cphExchange = cphConnection.CreateModel();
            cphExchange.ExchangeDeclare("cphbusiness.bankXML", "fanout");

            var consumer = new EventingBasicConsumer();
            consumer.Model = channel;
            consumer.Received += (s, e) =>
            {
                var msg = e.Body.ToRequestMessage<CreditScoreMessage>();

                var properties = new BasicProperties();
                properties.ReplyTo = "channel78";

                cphExchange.BasicPublish("cphbusiness.bankXML", "cphbusiness.bankXML", properties, string.Format(@"<LoanRequest>
<ssn>{0}</ssn>
<creditScore>{1}</creditScore>
<loanAmount>{2}</loanAmount>
<loanDuration>{3}</loanDuration>
</LoanRequest>", msg.Ssn, msg.CreditScore, msg.Amount, new DateTime(1970, 1, 1).AddDays(msg.Duration).ToString("yyyy-MM-dd " + "00:00:00.0 CET")).ToByteArray());
            };
            channel.BasicConsume("Bank1Requests", true, consumer);

            var cphConsumer = new EventingBasicConsumer();
            var cphChannel = cphConnection.CreateModel();
            cphChannel.QueueDeclare("channel78", true, false, false, null);
            cphConsumer.Model = cphChannel;

            cphConsumer.Received += (s, e) =>
                {
                    int i = 0;
                };
            cphChannel.BasicConsume("channel78", true, cphConsumer);
        }
    }
}
