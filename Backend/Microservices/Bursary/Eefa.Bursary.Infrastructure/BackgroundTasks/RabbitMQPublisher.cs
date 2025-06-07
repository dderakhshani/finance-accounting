using Eefa.Bursary.Application.Interfaces;
using RabbitMQ.Client;
using System.Text;

public class RabbitMQPublisher : IRabbitMQPublisher
{
    private readonly RabbitMQ.Client.IConnection _connection;

    public RabbitMQPublisher(RabbitMQ.Client.IConnection connection)
    {
        _connection = connection;
    }

    public void PublishFinancialEvent(string message)
    {
        // Create a channel
        using var channel = _connection.CreateModel();

        // 1. Declare the exchange (direct)
        const string exchangeName = "financial-events-exchange";
        channel.ExchangeDeclare(
            exchange: exchangeName,
            type: "direct",
            durable: true,
            autoDelete: false,
            arguments: null);

        // 2. (Optional) If you want to declare the queue here (the consumer can also do it)
        //    But typically, the consumer might declare it. We'll show how anyway:
        // const string queueName = "financial-events-queue";
        // channel.QueueDeclare(queue: queueName,
        //     durable: true,
        //     exclusive: false,
        //     autoDelete: false,
        //     arguments: null);
        //
        // // Bind the queue to the exchange with a routing key
        // channel.QueueBind(queue: queueName,
        //     exchange: exchangeName,
        //     routingKey: "financial-events-queue");

        // 3. Convert the message to bytes
        var body = Encoding.UTF8.GetBytes(message);

        // 4. Publish the message
        channel.BasicPublish(
            exchange: exchangeName,
            routingKey: "financial-events-queue", // must match consumer’s binding key
            basicProperties: null,
            body: body);
    }
}
