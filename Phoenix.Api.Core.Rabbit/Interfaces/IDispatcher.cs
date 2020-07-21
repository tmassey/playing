namespace Phoenix.Api.Core.RabbitClient.Interfaces
{
    public interface IDispatcher: IRabbitCore
    {
        void Publish(string routingKey, object message);
        void Publish(object message);
    }
}