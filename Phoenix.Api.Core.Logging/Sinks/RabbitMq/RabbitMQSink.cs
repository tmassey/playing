using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Phoenix.Api.Core.Logging.Enums;
using Phoenix.Api.Core.Logging.Models;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Raw;
using Serilog.Sinks.PeriodicBatching;

namespace Phoenix.Api.Core.Logging.Sinks.RabbitMq
{
    public class RabbitMQSink : PeriodicBatchingSink
    {
        private readonly ITextFormatter _formatter;
        private readonly IFormatProvider _formatProvider;
        private readonly RabbitMQClient _client;

        public RabbitMQSink(RabbitMQConfiguration configuration,
            ITextFormatter formatter,
            IFormatProvider formatProvider) : base(configuration.BatchPostingLimit, configuration.Period)
        {
            _formatter = formatter ?? new RawFormatter();
            _formatProvider = formatProvider;
            _client = new RabbitMQClient(configuration);
        }

        protected override void EmitBatch(IEnumerable<LogEvent> events)
        {
            foreach (var logEvent in events)
            {
                var sw = new StringWriter();
                _formatter.Format(logEvent, sw);
                switch (logEvent.Level)
                {
                    case LogEventLevel.Debug:
                        this.Debug(sw.ToString());
                        break;
                    case LogEventLevel.Fatal:
                        this.Fatal(logEvent.Exception);
                        break;
                    case LogEventLevel.Error:
                        this.Error(sw.ToString());
                        break;
                    case LogEventLevel.Information:
                        this.Info(sw.ToString());
                        break;
                    case LogEventLevel.Verbose:
                        this.Debug(sw.ToString());
                        break;
                    case LogEventLevel.Warning:
                        this.Warning(sw.ToString());
                        break;
                }
            }
        }
        public void Debug(string message, UserDetails userDetails = null)
        {
            Dispatch(GetMessage<PhoenixMessage>(message, Severity.Debug.ToString(), userDetails));
        }

        public void Info(string message, UserDetails userDetails = null)
        {
            if (ShouldSendInfo(message))
                Dispatch(GetMessage<PhoenixMessage>(message, Severity.Info.ToString(), userDetails));
        }

        private bool ShouldSendInfo(string message)
        {
            if (message.Contains(("Failed to authenticate HTTPS connection.")))
                return false;
            return true;
        }

        public void Warning(string message, UserDetails userDetails = null)
        {
            Dispatch(GetMessage<PhoenixMessage>(message, Severity.Warning.ToString(), userDetails));
        }

        public void Error(string message, UserDetails userDetails = null)
        {
            Dispatch(GetMessage<PhoenixMessage>(message, Severity.Error.ToString(), userDetails));
        }

        public void Network(string method, HttpStatusCode responseCode, string path, int duration, string serviceId = null, UserDetails userDetails = null)
        {
            var message = GetMessage<NetworkPhoenixMessage>(path, Severity.Network.ToString(), userDetails);
            message.HttpMethod = method;
            message.Duration = duration;
            message.ResponseCode = (int)responseCode;
            message.ResponseCodeText = $"{responseCode}";
            message.ServiceId = serviceId;
            Dispatch(message);
        }

        public void Fatal(Exception ex, UserDetails userDetails = null)
        {
            var inner = ex;
            while (inner != null && inner.InnerException != null) inner = inner.InnerException;
            Dispatch(GetMessage<PhoenixMessage>($"{inner}", Severity.Fatal.ToString(), userDetails));
        }

        private void Dispatch(PhoenixMessage PhoenixMessage)
        {
            _client.Publish(JsonConvert.SerializeObject(PhoenixMessage));
            //_logDispatcher.Publish($"Phoenix.{PhoenixMessage.Severity}.Message", PhoenixMessage);
        }

        private static T GetMessage<T>(string message, string severity, UserDetails userDetails) where T : PhoenixMessage, new()
        {
            //Pop previous call off the call stack to get trace info from caller
            var frame = new StackFrame(2, true);
            return new T
            {
                MachineName = Environment.MachineName,
                ApplicationName = Process.GetCurrentProcess().ProcessName,
                AssemblyName = frame.GetFileName(),
                MethodName = frame.GetMethod().ToString(),
                LineNumber = frame.GetFileLineNumber(),
                Severity = severity,
                MessageText = message,
                UserEmail = userDetails?.Email ?? "",
                UserId = userDetails?.Id.ToString() ?? "",
                ClientId = userDetails?.ClientId ?? "",
                UserName = userDetails?.UserName ?? ""
            };
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _client.Dispose();
        }
    }
}