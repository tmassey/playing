using System;
using System.Timers;
using playing.Core.TimerFactory.Interfaces;

namespace playing.Core.TimerFactory
{
    public class ServiceTimer: IServiceTimer
    {
        private Timer _timer;
        public bool IsRunning { get; private set; }
        public bool IsEnabled => _timer.Enabled;

        public void Init(int interval, ElapsedEventHandler elapsedEventHandler)
        {
            _timer = new Timer(interval);
            _timer.Elapsed += elapsedEventHandler;
        }

        public void Start()
        {
            if (_timer == null) throw new Exception("Service time not initialized.");
            IsRunning = true;
            _timer.Start();
        }

        public void Stop()
        {
            IsRunning = false;            
            _timer.Stop();
        }

        public void Disable()
        {
            _timer.Enabled = false;
        }

        public void Enable()
        {
            _timer.Enabled = true;
        }

        public void Dispose()
        {
            if (_timer == null) return;
            _timer.Stop();
            _timer.Dispose();
        }
    }
}