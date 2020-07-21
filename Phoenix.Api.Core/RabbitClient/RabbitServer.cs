using System.Collections.Generic;
using Phoenix.Api.Core.RabbitClient.Interfaces;
using Phoenix.Api.Core.RabbitClient.Models;
using RabbitMQ.Client;

namespace Phoenix.Api.Core.RabbitClient
{
    public sealed class RabbitServer : IRabbitServer
    {
        private IConnectionFactory _connectionFactory;
        private IConnectionWrapper _connection;
        

        public RabbitServer(string hostName, string userName, string password, int port = 5672)
        {
            CreateConnection(hostName, userName, password, port);
        } 

        private void CreateConnection(string hostName, string userName, string password, int port = 5672)
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = hostName,
                Port = port,
                UserName = userName,
                Password = password,
                AutomaticRecoveryEnabled = true
            };
            
            _connection = new ConnectionWrapper(_connectionFactory.CreateConnection());   
        }

        public IConnectionWrapper GetConnection()
        {
            return _connection;
        }

        public IListener CreateDirectExchangeListener(string name, bool autoDelete = true, IDictionary<string, object> arguments = null)
        {
            return CreateListenerInstance(name, DispatchType.Direct, autoDelete, arguments);
        }

        public IListener CreateFanoutExchangeListener(string name, bool autoDelete = true, IDictionary<string, object> arguments = null)
        {
            return CreateListenerInstance(name, DispatchType.Fanout, autoDelete, arguments);
        }

        public IListener CreateTopicExchangeListener(string name, bool autoDelete = true, IDictionary<string, object> arguments = null, string topicFilter = "#")
        {
            return CreateListenerInstance(name, DispatchType.Topic, autoDelete, arguments, topicFilter);
        }

        public IListener CreateWorkerQueueListener(string name, bool autoDelete = true, IDictionary<string, object> arguments = null)
        {
            return CreateListenerInstance(name, DispatchType.Worker, autoDelete, arguments);
        }

        private Listener CreateListenerInstance(string name, DispatchType type, bool autoDelete, IDictionary<string, object> arguments, string topicFilter = "#")
        {
            var listener = new Listener(GetConnection(), name, type, autoDelete, arguments, topicFilter);
            return listener;
        }

        public IDispatcher CreateDirectExchangeDispatcher(string name, IDictionary<string, object> arguments = null)
        {
            return CreateDispatcher(name, DispatchType.Direct, false, arguments);
        }

        public IDispatcher CreateFanoutExchangeDispatcher(string name, IDictionary<string, object> arguments = null)
        {
            return CreateDispatcher(name, DispatchType.Fanout, false, arguments);
        }

        public IDispatcher CreateTopicExchangeDispatcher(string name, IDictionary<string, object> arguments = null)
        {
            return CreateDispatcher(name, DispatchType.Topic, false, arguments);
        }

        public IDispatcher CreateWorkerQueueDispatcher(string name, bool autoDelete = true, IDictionary<string, object> arguments = null)
        {
            return CreateDispatcher(name, DispatchType.Worker, autoDelete, arguments);
        }

        private Dispatcher CreateDispatcher(string name, DispatchType type, bool autoDelete, IDictionary<string, object> arguments)
        {
            var dispatcher = new Dispatcher(GetConnection(), name, type, autoDelete, arguments);
            return dispatcher;
        }

        public void Dispose()
        {
            if (_connection == null) return;
            if (_connection.IsOpen) _connection.Close();
            _connection.Dispose();
        }
    }
}