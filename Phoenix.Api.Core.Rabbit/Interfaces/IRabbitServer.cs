using System;
using System.Collections.Generic;

namespace Phoenix.Api.Core.RabbitClient.Interfaces
{
    public interface IRabbitServer: IDisposable
    {
        IConnectionWrapper GetConnection();
        IListener CreateDirectExchangeListener(string name, bool autoDelete = true, IDictionary<string, object> arguments = null);
        IListener CreateFanoutExchangeListener(string name, bool autoDelete = true, IDictionary<string, object> arguments = null);
        IListener CreateTopicExchangeListener(string name, bool autoDelete = true, IDictionary<string, object> arguments = null, string topicFilter = "#");
        IListener CreateWorkerQueueListener(string name, bool autoDelete = true, IDictionary<string, object> arguments = null);
        IDispatcher CreateDirectExchangeDispatcher(string name, IDictionary<string, object> arguments = null);
        IDispatcher CreateFanoutExchangeDispatcher(string name, IDictionary<string, object> arguments = null);
        IDispatcher CreateTopicExchangeDispatcher(string name, IDictionary<string, object> arguments = null);
        IDispatcher CreateWorkerQueueDispatcher(string name, bool autoDelete = true, IDictionary<string, object> arguments = null);
    }
}
