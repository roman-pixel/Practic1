using System;
using System.Text;
using RabbitMQ.Client;

namespace Practic1
{
    internal class Program
    {
        #region Создание очеридей и отпрвка туда сообщений
        private static void MessQueue(string message)
        {
            string digit = "";
            string str = "";
            bool isEven = false;

            foreach (char ch in message)
            {
                if (char.IsDigit(ch))
                {
                    digit += ch;
                }
                if (char.IsLetter(ch))
                {
                    str += ch;
                }
            }

            if (digit != "")
            {
                isEven = Convert.ToInt32(digit) % 2 == 0 ? true : false;
            }

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "numEven", durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueDeclare(queue: "numOdd", durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueDeclare(queue: "notNum", durable: false, exclusive: false, autoDelete: false, arguments: null);

                if (str != "")
                {
                    var body = Encoding.UTF8.GetBytes(str);
                    channel.BasicPublish(exchange: "", routingKey: "notNum", basicProperties: null, body: body);
                }
                if (digit != "")
                {
                    if (isEven)
                    {
                        var body = Encoding.UTF8.GetBytes(digit);
                        channel.BasicPublish(exchange: "", routingKey: "numEven", basicProperties: null, body: body);
                    }
                    else
                    {
                        var body = Encoding.UTF8.GetBytes(digit);
                        channel.BasicPublish(exchange: "", routingKey: "numOdd", basicProperties: null, body: body);
                    }
                }

                Console.WriteLine($" [x] Sent {message}");
            }
        }
        #endregion

        static void Main(string[] args)
        {
            while (true)
            {
                //string message = "Hello World!";
                Console.WriteLine("Enter your message:");
                var message = Console.ReadLine();

                MessQueue(message);
            }
            //Console.WriteLine(" Press [enter] to exit.");
            //Console.ReadLine();
        }
    }
}
