using System;
using System.Collections.Generic;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Phoenix.Api.Core.RabbitClient.Interfaces
{
    public interface IModelWrapper : IDisposable
    {
        void ExchangeDeclare(string name, string type, bool durable, bool autoDelete, IDictionary<string, object> arguments);
        QueueDeclareOk QueueDeclare(string queue, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> arguments);
        QueueDeclareOk QueueDeclare();
        void QueueBind(string queue, string exchange, string routingKey, IDictionary<string, object> arguments = null);
        string BasicConsume(string queue, bool noAck, EventingBasicConsumer consumer);
        void BasicPublish(string exchange, string routingKey, IBasicProperties basicProperties, byte[] body);
        bool IsOpen { get; }
        void Close(ushort replyCode, string replyText);
        IModel Model { get; }
    }
}