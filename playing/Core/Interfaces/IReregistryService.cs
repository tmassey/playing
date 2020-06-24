using System;
using System.Timers;

namespace playing.Core.Interfaces
{
    public interface IReregistryService : IDisposable
    {
        void Start();
        void Stop();
        void Reset();
        void TimeoutReached(object sender, ElapsedEventArgs e);
    }
}