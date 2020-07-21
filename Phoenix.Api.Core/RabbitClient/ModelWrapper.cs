using System.Collections.Generic;
using Phoenix.Api.Core.RabbitClient.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Phoenix.Api.Core.RabbitClient
{
    public class ModelWrapper : IModelWrapper
    {
        public IModel Model { get; }

        public ModelWrapper(IModel model)
        {
            Model = model;
        }

        public void ExchangeDeclare(string name, string type, bool durable, bool autoDelete, IDictionary<string, object> arguments)
        {
            Model.ExchangeDeclare(name, type, durable, autoDelete, arguments);
        }

        public string BasicConsume(string queue, bool noAck, EventingBasicConsumer consumer)
        {
            return  Model.BasicConsume(queue, noAck, consumer);
        }

        public QueueDeclareOk QueueDeclare(string queue, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> arguments)
        {
            return Model.QueueDeclare(queue, durable, exclusive, autoDelete, arguments);
        }

        public QueueDeclareOk QueueDeclare()
        {
            return Model.QueueDeclare();
        }

        public void BasicPublish(string exchange, string routingKey, IBasicProperties basicProperties, byte[] body)
        {
            Model.BasicPublish(exchange, routingKey, basicProperties, body);
        }

        public void QueueBind(string queue, string exchange, string routingKey, IDictionary<string, object> arguments = null)
        {
            Model.QueueBind(queue, exchange, routingKey, arguments);
        }

        public bool IsOpen => Model.IsOpen;

        public void Close(ushort replyCode, string replyText)
        {
            Model.Close(replyCode, replyText);
        }

        public void Dispose()
        {
            Model.Dispose();
        }
    }
}
