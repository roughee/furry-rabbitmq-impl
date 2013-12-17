using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var connection2 = factory.CreateConnection();

            var channel = connection.CreateModel();
            channel.QueueDeclare("Bank1Requests", true, false, false, null);

            var channel2 = connection2.CreateModel();
            channel2.QueueDeclare("cphbusiness.bankXML", false, false, false, null);

            var consumer = new EventingBasicConsumer();
            consumer.Model = channel;
            consumer.Received += (s, e) =>
            {
                var msg = e.Body.ToRequestMessage<CreditScoreMessage>();

                channel2.BasicPublish("cphbusiness.bankXML", "cphbusiness.bankXML", new BasicProperties { Headers = new Dictionary<string, object> { { "reply-to", "channel78"} } }, string.Format(@"<LoanRequest>
<ssn>{0}</ssn>
<creditScore>{1}</creditScore>
<loanAmount>{2}</loanAmount>
<loanDuration>{3}</loanDuration>
</LoanRequest>", msg.Ssn, msg.CreditScore, msg.Amount, new DateTime(1970).AddDays(msg.Duration).ToString("yyyy-MM-dd " + "00:00:00.0 CET")).ToByteArray());
            };
            channel.BasicConsume("Bank1Requests", true, consumer);
        }
    }
}
