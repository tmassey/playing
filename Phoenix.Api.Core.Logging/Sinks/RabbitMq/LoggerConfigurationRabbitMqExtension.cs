﻿using System;
using RabbitMQ.Client;
using Serilog;
using Serilog.Configuration;
using Serilog.Formatting;
using Serilog.Sinks.RabbitMQ;

namespace Phoenix.Api.Core.Logging.Sinks.RabbitMq
{
    public static class LoggerConfigurationRabbitMqExtension
    {
        /// <summary>
        /// Adds a sink that lets you push log messages to RabbitMq
        /// </summary>
        public static LoggerConfiguration RabbitMQ(
            this LoggerSinkConfiguration loggerConfiguration,
            RabbitMQConfiguration rabbitMqConfiguration,
            ITextFormatter formatter,
            IFormatProvider formatProvider = null)
        {
            if (loggerConfiguration == null) throw new ArgumentNullException("loggerConfiguration");
            if (rabbitMqConfiguration == null) throw new ArgumentNullException("rabbitMqConfiguration");

            // calls overloaded extension method
            return loggerConfiguration.RabbitMQ(
                rabbitMqConfiguration.Hostname,
                rabbitMqConfiguration.Username,
                rabbitMqConfiguration.Password,
                rabbitMqConfiguration.Exchange,
                rabbitMqConfiguration.ExchangeType,
                rabbitMqConfiguration.DeliveryMode,
                rabbitMqConfiguration.RouteKey,
                rabbitMqConfiguration.Port,
                rabbitMqConfiguration.VHost,
                rabbitMqConfiguration.Heartbeat,
                rabbitMqConfiguration.Protocol,
                rabbitMqConfiguration.BatchPostingLimit,
                rabbitMqConfiguration.Period,
                formatter,
                formatProvider);
        }

        /// <summary>
        /// Configures Serilog logger configuration with RabbitMQ
        /// </summary>
        public static LoggerConfiguration RabbitMQ(
            this LoggerSinkConfiguration loggerConfiguration,
            string hostname,
            string username,
            string password,
            string exchange = null,
            string exchangeType = null,
            RabbitMQDeliveryMode deliveryMode = RabbitMQDeliveryMode.NonDurable,
            string routeKey = null,
            int port = 0,
            string vHost = null,
            ushort heartbeat = 0,
            IProtocol protocol = null,
            int batchPostingLimit = 0,
            TimeSpan period = default(TimeSpan),
            ITextFormatter formatter = null,
            IFormatProvider formatProvider = null)
        {
            // guards
            if (loggerConfiguration == null) throw new ArgumentNullException("loggerConfiguration");
            if (string.IsNullOrEmpty(hostname)) throw new ArgumentException("hostname cannot be 'null'. Enter a valid hostname.");
            if (string.IsNullOrEmpty(username)) throw new ArgumentException("username cannot be 'null' or and empty string.");
            if (password == null) throw new ArgumentException("password cannot be 'null'. Specify an empty string if password is empty.");
            if (port < 0 || port > 65535) throw new ArgumentOutOfRangeException("port", "port must be in a valid range (1 and 65535 or 0 for default)");

            // setup configuration
            var config = new RabbitMQConfiguration
            {
                Hostname = hostname,
                Username = username,
                Password = password,
                Exchange = exchange ?? string.Empty,
                ExchangeType = exchangeType ?? string.Empty,
                DeliveryMode = deliveryMode,
                RouteKey = routeKey ?? string.Empty,
                Port = port,
                VHost = vHost ?? string.Empty,
                Protocol = protocol ?? Protocols.DefaultProtocol,
                Heartbeat = heartbeat,
                BatchPostingLimit = batchPostingLimit == default(int) ? DefaultBatchPostingLimit : batchPostingLimit,
                Period = period == default(TimeSpan) ? DefaultPeriod : period
            };

            return
                loggerConfiguration
                    .Sink(new RabbitMQSink(config, formatter, formatProvider));
        }

        private const int DefaultBatchPostingLimit = 50;
        private static readonly TimeSpan DefaultPeriod = TimeSpan.FromSeconds(2);
    }
}