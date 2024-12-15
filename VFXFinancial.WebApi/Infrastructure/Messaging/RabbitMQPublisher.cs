using RabbitMQ.Client;
using System.Text;

namespace VFXFinancial.WebApi.Infrastructure.Messaging
{
    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        private readonly IModel _channel;
        private readonly ILogger<RabbitMQPublisher> _logger;

        public RabbitMQPublisher(IModel channel, ILogger<RabbitMQPublisher> logger)
        {
            _channel = channel;
            _logger = logger;
        }

        public async Task PublishAsync(string queueName, string message)
        {
            try
            {
                // Ensure the queue exists
                _channel.QueueDeclare(queue: queueName,
                                      durable: true,
                                      exclusive: false,
                                      autoDelete: false,
                                      arguments: null);

                // Convert the message to byte array
                var body = Encoding.UTF8.GetBytes(message);

                // Publish the message
                _channel.BasicPublish(exchange: "",
                                      routingKey: queueName,
                                      basicProperties: null,
                                      body: body);

                _logger.LogInformation("Published message to queue '{QueueName}': {Message}", queueName, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing message to queue '{QueueName}'", queueName);
                throw;
            }
        }
    }
}
