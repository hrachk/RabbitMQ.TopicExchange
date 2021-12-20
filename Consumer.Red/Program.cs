using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Consumer Red!");


var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.ExchangeDeclare(exchange: "topic_logs", ExchangeType.Topic);

    var queueName = channel.QueueDeclare().QueueName;

    channel.QueueBind(queue: queueName,
                     exchange: "topic_logs",
                     routingKey: "*.red.#");

    var consumer = new EventingBasicConsumer(channel);

    consumer.Received += (sender, args) =>
    {
        var body = args.Body;
        var message = Encoding.UTF8.GetString(body.ToArray());
        Console.WriteLine($"Received message: {message} ");

    };

    channel.BasicConsume(queue: queueName,
        autoAck: true,
        consumer: consumer);
    Console.WriteLine($"Subscribed to the '{queueName}'");
    Console.WriteLine($"Listening to   [*.red.#]");
    Console.ReadLine();
}