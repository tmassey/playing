using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Phoenix.Api.Core.RabbitClient.Interfaces;
using Phoenix.Api.Core.RabbitClient.Models;

namespace Phoenix.Api.Core.RabbitClient
{
    public class Dispatcher: RabbitCore, IDispatcher
    {
        public Dispatcher(IConnectionWrapper connection, string name, DispatchType dispatchType, bool autoDelete, IDictionary<string, object> arguments = null) : 
            base(connection, dispatchType, name, autoDelete, arguments)
        {
        }

        public void Publish(object message)
        {
            Publish(Name, message);
        }

        public void Publish(string routingKey, object message)
        {
            var transaction = BuildTransaction(routingKey, message);
            var binaryPayload = Encoding.UTF8.GetBytes(transaction.payload);
            var properties = ModelWrapper.Model.CreateBasicProperties();
            properties.CorrelationId = transaction.message_id.ToString();

            ModelWrapper.BasicPublish(
                transaction.queue_name, 
                transaction.routing_key,
                properties, 
                binaryPayload);                        
        }

        private RabbitTransaction BuildTransaction(string routingKey, object message)
        {
            var messagePayload = JsonConvert.SerializeObject(message);
            return new RabbitTransaction
            {
                message_id = Guid.NewGuid(),
                payload = messagePayload,
                queue_name = IsExchange ? Name : string.Empty,
                routing_key = routingKey ?? string.Empty
            };
        }
        
    }
}
