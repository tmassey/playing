﻿using System;
using RabbitMQ.Client;

namespace Phoenix.Api.Core.Logging.Sinks.RabbitMq
{
    public class RabbitMQClient : IDisposable
    {
        // configuration member
        private readonly RabbitMQConfiguration _config;
        private readonly PublicationAddress _publicationAddress;

        // endpoint members
        private IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _model;
        private IBasicProperties _properties;

        /// <summary>
        /// Constructor for RabbitMqClient
        /// </summary>
        /// <param name="configuration">mandatory</param>
        public RabbitMQClient(RabbitMQConfiguration configuration)
        {
            // load configuration
            _config = configuration;
            _publicationAddress = new PublicationAddress(_config.ExchangeType, _config.Exchange, _config.RouteKey);

            // initialize 
            InitializeEndpoint();
        }

        /// <summary>
        /// Private method, that must be run for the client to work.
        /// <remarks>See constructor</remarks>
        /// </summary>
        private void InitializeEndpoint()
        {
            // prepare endpoint
            _connectionFactory = GetConnectionFactory();
            _connection = _connectionFactory.CreateConnection();
            _model = _connection.CreateModel();

            _properties = _model.CreateBasicProperties();
            _properties.DeliveryMode = (byte)_config.DeliveryMode; //persistance
        }

        /// <summary>
        /// Configures a new ConnectionFactory, and returns it
        /// </summary>
        /// <returns></returns>
        private IConnectionFactory GetConnectionFactory()
        {
            // prepare connection factory
            var connectionFactory = new ConnectionFactory
            {
                HostName = _config.Hostname,
                UserName = _config.Username,
                Password = _config.Password,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(2)
            };

            // setup heartbeat if needed
            if (_config.Heartbeat > 0)
                connectionFactory.RequestedHeartbeat = _config.Heartbeat;

            // only set, if has value, otherwise leave default
            if (_config.Port > 0) connectionFactory.Port = _config.Port;
            if (!string.IsNullOrEmpty(_config.VHost)) connectionFactory.VirtualHost = _config.VHost;
            if (_config.Protocol != null) connectionFactory.Protocol = _config.Protocol;

            // return factory
            return connectionFactory;
        }

        /// <summary>
        /// Publishes a message to RabbitMq Exchange
        /// </summary>
        /// <param name="message"></param>
        public void Publish(string message)
        {
            // push message to exchange
            _model.BasicPublish(_publicationAddress, _properties, System.Text.Encoding.UTF8.GetBytes(message));
        }

        public void Dispose()
        {
            _model.Dispose();
            _connection.Dispose();
        }
    }
}