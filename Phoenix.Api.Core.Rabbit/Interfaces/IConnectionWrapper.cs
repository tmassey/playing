using System;

namespace Phoenix.Api.Core.RabbitClient.Interfaces
{
    public interface IConnectionWrapper: IDisposable
    {
        IModelWrapper CreateModel();
        bool IsOpen { get; }
        void Close();
    }
}