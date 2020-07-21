using System;

namespace Phoenix.Api.Core.RabbitClient.Interfaces
{
    public interface IRabbitCore : IDisposable
    {
        uint MessageCount { get; }
        uint ConsumerCount { get; }
    }
}