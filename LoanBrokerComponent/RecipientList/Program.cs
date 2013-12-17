using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RecipientList
{
        class Program
        {
            private static Dictionary<string, IModel> _channels;
            static IConnection _connection;
            static Program()
            {
                _channels = new Dictionary<string, IModel>();
                _connection = new ConnectionFactory

                {

                    HostName = "localhost",

                    UserName = "guest",

                    Password = "guest",

                }.CreateConnection();
                foreach (var bank in ConfigurationManager.AppSettings.AllKeys)
                {
                    var channel = _connection.CreateModel();

                    var channelName = ConfigurationManager.AppSettings[bank];
                    channel.QueueDeclare(channelName, true, false, false, null);
                    _channels.Add(channelName, channel);
                }
            }


            static void Main(string[] args)
            {
                var channel = _connection.CreateModel();

                channel.QueueDeclare("BanksList", true, false, false, null);


                var consumer = new EventingBasicConsumer();

                consumer.Model = channel;

                consumer.Received += (s, e) =>
                {

                    var message = e.Body.ToRequestMessage<BanksListMessage>();



                    foreach (var bankName in message.BanksList)
                    {

                        var bankChannelName = ConfigurationManager.AppSettings[bankName].ToString();

                        _channels[bankChannelName].BasicPublish("", bankChannelName, null, message.ToByteArray());
                    }

                };

                channel.BasicConsume("BanksList", true, consumer);

            }

        }
}
