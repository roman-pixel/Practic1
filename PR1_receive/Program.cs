using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Text;

namespace PR1_receive
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "numEven", durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueDeclare(queue: "numOdd", durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueDeclare(queue: "notNum", durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumerNumEven = new EventingBasicConsumer(channel);
                var consumerNumOdd = new EventingBasicConsumer(channel);
                var consumerNotNum = new EventingBasicConsumer(channel);

                consumerNumEven.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($" [x] Received even digit {message}");
                };

                consumerNumOdd.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($" [x] Received odd digit {message}");
                };

                consumerNotNum.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($" [x] Received string {message}");
                };

                channel.BasicConsume(queue: "numEven", autoAck: true, consumer: consumerNumEven);
                channel.BasicConsume(queue: "numOdd", autoAck: true, consumer: consumerNumOdd);
                channel.BasicConsume(queue: "notNum", autoAck: true, consumer: consumerNotNum);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
