using System;
using System.Text;
using RabbitMQ.Client;

namespace Practic1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                //string message = "Hello World!";
                Console.WriteLine("Enter your message:");
                var message = Console.ReadLine();

                int Num;
                bool isNum = int.TryParse(message, out Num);

                var queueChoice = "";

                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {

                    if (isNum)
                    {
                        queueChoice = "num";
                    }
                    else
                    {
                        queueChoice = "notNum";
                    }

                    channel.QueueDeclare(queue: queueChoice,
                                                         durable: false,
                                                         exclusive: false,
                                                         autoDelete: false,
                                                         arguments: null);

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: queueChoice,
                                         basicProperties: null,
                                         body: body);
                    Console.WriteLine(" [x] Sent {0}", message);
                }

            }
            //Console.WriteLine(" Press [enter] to exit.");
            //Console.ReadLine();
        }
    }
}
