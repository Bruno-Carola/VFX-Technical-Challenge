using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace VFXFinancial.WebApi.Infrastructure.Messaging
{
    public class RabbitMQConsumer
    {
        private readonly IModel _channel;
        private readonly ILogger<RabbitMQConsumer> _logger;

        public RabbitMQConsumer(IModel channel, ILogger<RabbitMQConsumer> logger)
        {
            _channel = channel;
            _logger = logger;
        }

        public async Task StartListeningAsync(string queueName)
        {
            try
            {
                _channel.QueueDeclare(queue: queueName,
                                  durable: true,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    _logger.LogInformation("Received message from queue '{QueueName}': {Message}", queueName, message);

                    // Acknowledge the message
                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };

                _channel.BasicConsume(queue: queueName,
                                      autoAck: false,
                                      consumer: consumer);

                _logger.LogInformation("Started listening on queue '{QueueName}'", queueName);

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
