// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Consumer Ecological!");


var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.ExchangeDeclare(exchange: "topic_logs", ExchangeType.Topic);

    var queueName = channel.QueueDeclare().QueueName;

    channel.QueueBind(queue:queueName,
                     exchange: "topic_logs",
                     routingKey: "*.*.*.ecological");

    var consumer = new EventingBasicConsumer(channel);

    consumer.Received += (sender, args) =>
    {
        var body = args.Body;
        var message = Encoding.UTF8.GetString(body.ToArray());
        Console.WriteLine($"Received message: {message} ");

    };

    channel.BasicConsume(queue:queueName,
        autoAck:true,
        consumer:consumer);
    Console.WriteLine($"Subscribed to the '{queueName}'");
    Console.WriteLine($"Listening to   [*.*.*.ecological]");
    Console.ReadLine();
}