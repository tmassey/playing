using System;
using RabbitMQ.Client.Events;

namespace Phoenix.Api.Core.RabbitClient.Interfaces
{
    public interface IListener: IRabbitCore
    {
        void BindDelegateToRoutingKey(string routingKey, RabbitCore.RabbitMessageDelegate handler);
        void BindDelegateToQueue(RabbitCore.RabbitMessageDelegate handler);
        event Action<dynamic, object, BasicDeliverEventArgs> MessageReceived;
    }
}