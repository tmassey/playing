using System;
using System.Diagnostics;
using System.Net;
using Phoenix.Api.Core.Logging.Enums;
using Phoenix.Api.Core.Logging.Interfaces;
using Phoenix.Api.Core.Logging.Models;
using Phoenix.Api.Core.RabbitClient.Interfaces;

namespace Phoenix.Api.Core.Logging.RabbitLogger
{
    public class RabbitLogger : PhoenixLoggerBase, IPhoenixLogger
    {
        private static IRabbitServer _rabbitServer;
        private static IDispatcher _logDispatcher;
        public RabbitLogger(IRabbitServer rabbitServer, string exchangeName = "Logs")
        {
            _rabbitServer = rabbitServer;
            _logDispatcher = rabbitServer.CreateTopicExchangeDispatcher(exchangeName);
        }

        public void Debug(string message, UserDetails userDetails = null)
        {
            if (LogLevel < Severity.Debug) return;
            Dispatch(GetMessage<PhoenixMessage>(message, Severity.Debug.ToString(), userDetails));
        }

        public void Info(string message, UserDetails userDetails = null)
        {
            if (LogLevel < Severity.Info) return;
            Dispatch(GetMessage<PhoenixMessage>(message, Severity.Info.ToString(), userDetails));
        }

        public void Warning(string message, UserDetails userDetails = null)
        {
            if (LogLevel < Severity.Warning) return;
            Dispatch(GetMessage<PhoenixMessage>(message, Severity.Warning.ToString(), userDetails));
        }

        public void Error(string message, UserDetails userDetails = null)
        {
            if (LogLevel < Severity.Error) return;
            Dispatch(GetMessage<PhoenixMessage>(message, Severity.Error.ToString(), userDetails));
        }

        public void Network(string method, HttpStatusCode responseCode, string path, int duration, string serviceId = null, UserDetails userDetails = null)
        {
            if (LogLevel < Severity.Network) return;
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
            while (inner.InnerException != null) inner = inner.InnerException;              
            Dispatch(GetMessage<PhoenixMessage>($"{inner}", Severity.Fatal.ToString(), userDetails));
        }

        private static void Dispatch(PhoenixMessage PhoenixMessage)
        {
            _logDispatcher.Publish($"Phoenix.{PhoenixMessage.Severity}.Message", PhoenixMessage);
        }

        private static T GetMessage<T>(string message, string severity, UserDetails userDetails) where T: PhoenixMessage, new()
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

        public void Dispose()
        {
            _logDispatcher.Dispose();
            _rabbitServer.Dispose();
        }


    }
}
