using System;
using System.Timers;

namespace Phoenix.Api.Core.RegistryService.Interfaces
{
    public interface IRegistryService : IDisposable
    {
        void Start();
        void Stop();
        void Reset();
        void TimeoutReached(object sender, ElapsedEventArgs e);
    }
}