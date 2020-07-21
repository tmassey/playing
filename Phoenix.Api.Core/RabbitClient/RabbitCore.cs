using System.Collections.Generic;
using Newtonsoft.Json;
using Phoenix.Api.Core.RabbitClient.Interfaces;
using Phoenix.Api.Core.RabbitClient.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Phoenix.Api.Core.RabbitClient
{
    public abstract class RabbitCore: IRabbitCore
    {
        protected DispatchType DispatchType;
        protected string Name;
        protected readonly bool AutoDelete;
        protected IConnectionWrapper Connection;
        protected IModelWrapper ModelWrapper;
        protected IDictionary<string, object> Arguments;
        public delegate void RabbitMessageDelegate(dynamic data, object sender, BasicDeliverEventArgs basicDeliverEventArgs);
        
        protected bool IsExchange => DispatchType == DispatchType.Direct ||
                                     DispatchType == DispatchType.Fanout ||
                                     DispatchType == DispatchType.Topic;

        public uint ConsumerCount { get; private set; }
        public uint MessageCount { get; private set; }

        private readonly Dictionary<DispatchType, string> _exchangeTypes = new Dictionary<DispatchType, string>
        {
            { DispatchType.Topic, ExchangeType.Topic },
            { DispatchType.Direct, ExchangeType.Direct },
            { DispatchType.Fanout, ExchangeType.Fanout }
        };

        protected RabbitCore(IConnectionWrapper connection, DispatchType dispatchType, string name, bool autoDelete, IDictionary<string, object> arguments = null)
        {
            DispatchType = dispatchType;
            Name = name;
            Connection = connection;
            AutoDelete = autoDelete;
            Arguments = arguments;

            ModelWrapper = Connection.CreateModel();
            SetupQueue();
        }

       private void SetupQueue()
        {
            if (IsExchange)
                CreateExchange();
            else
                CreateQueue();
        }

        protected void CreateExchange()
        {
            ModelWrapper.ExchangeDeclare(
                Name, 
                _exchangeTypes[DispatchType],
                false, 
                false,
                Arguments);
        }

        protected void CreateQueue()
        {
            var qd = ModelWrapper.QueueDeclare(Name, false, false, AutoDelete, Arguments);
            ConsumerCount = qd.ConsumerCount;
            MessageCount = qd.MessageCount;
        }

        public virtual void Dispose()
        {
            if (ModelWrapper == null) return;
            if (ModelWrapper.IsOpen) ModelWrapper.Close(200, "Goodbye");
            ModelWrapper.Dispose();
        }

        protected static dynamic ConvertData(string data)
        {
            try
            {
                return JsonConvert.DeserializeObject(data);
            }
            catch (JsonException e)
            {
                Service.PhoenixLogger.Fatal(e);
                return data;
            }
        }
    }
}
