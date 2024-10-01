using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

//реализация простого вычитывания из очереди rabbitmq

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory { HostName = "localhost", Port = 5672, UserName = "rxuser", Password = "rxuser", VirtualHost = "rxhost" };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            var consumer = new EventingBasicConsumer(channel);
            
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);
            };

            channel.BasicConsume(queue: "q1",
                                 autoAck: false,
                                 consumer: consumer);

            Console.WriteLine("Enter to exit.");
            Console.ReadLine();
        }
    }
}
