using System;
using System.Timers;

namespace Phoenix.Api.Core.TimerFactory.Interfaces
{
    public interface IServiceTimer: IDisposable
    {
        void Init(int interval, ElapsedEventHandler elapsedEventHandler);
        void Disable();
        void Enable();
        void Start();
        void Stop();
        bool IsEnabled { get; }
        bool IsRunning { get; }
    }
}