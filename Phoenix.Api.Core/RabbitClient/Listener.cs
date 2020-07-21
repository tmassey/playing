using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phoenix.Api.Core.RabbitClient.Interfaces;
using Phoenix.Api.Core.RabbitClient.Models;
using RabbitMQ.Client.Events;

namespace Phoenix.Api.Core.RabbitClient
{
    public class Listener: RabbitCore, IListener
    {
        private string _queueName;
        private EventingBasicConsumer _consumer;
        private readonly Dictionary<string, Delegate> _delegates = new Dictionary<string, Delegate>();
        private readonly string _topicFilter;
        
        public event Action<dynamic, object, BasicDeliverEventArgs> MessageReceived;

        public Listener(IConnectionWrapper connection, string name, DispatchType dispatchType, bool autoDelete, IDictionary<string, object> arguments = null, string topicFilter = "#") :
            base(connection, dispatchType, name, autoDelete, arguments)
        {
            _topicFilter = topicFilter;

            CreateConsumer();
            BindQueueToExchange();
        }

        public void BindDelegateToQueue(RabbitMessageDelegate handler)
        {
            _delegates.Add(Name, handler);
        }

        public void BindDelegateToRoutingKey(string routingKey, RabbitMessageDelegate handler)
        {
            _delegates.Add(routingKey, handler);
        }

        private void CreateConsumer()
        {
            _queueName = IsExchange ?  ModelWrapper.QueueDeclare().QueueName : Name;
            _consumer = new EventingBasicConsumer(ModelWrapper.Model);
            ModelWrapper.BasicConsume(_queueName, true, _consumer);
            _consumer.Received += Callback;
        }

        private void BindQueueToExchange()
        {
            if (!IsExchange)  return;
            if (DispatchType == DispatchType.Topic)
                BindWithTopicFilters();
            else
                ModelWrapper.QueueBind(_queueName, Name, ""); 
        }

        private void BindWithTopicFilters()
        {
            foreach (var filter in _topicFilter.Split(';'))
            {
                ModelWrapper.QueueBind(_queueName, Name, filter);
            }
        }

        private void Callback(object sender, BasicDeliverEventArgs args)
        {
            var message = Encoding.UTF8.GetString(args.Body);
            var binaryMessage = ConvertData(message);

            OnMessageReceived(binaryMessage, sender, args);
            CallDelegates(sender, args, binaryMessage);           
        }

        private void CallDelegates(object sender, BasicDeliverEventArgs args, dynamic convertedData)
        {
            if (_delegates.All(r => r.Key != args.RoutingKey)) return;
            var handler = _delegates[args.RoutingKey];
            handler.DynamicInvoke(convertedData, sender, args);
        }

        protected virtual void OnMessageReceived(dynamic data, object sender, BasicDeliverEventArgs args)
        {
            MessageReceived?.Invoke(data, sender, args);
        }
    }
}