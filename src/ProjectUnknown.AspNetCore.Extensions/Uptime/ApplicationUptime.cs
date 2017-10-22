using System;

namespace ProjectUnknown.AspNetCore.Extensions.Uptime
{
    public class ApplicationUptime : IApplicationUptime
    {
        private bool _started;
        private bool _stopped;
        private DateTimeOffset _startedAt;
        private DateTimeOffset _stoppedAt;

        public void Start()
        {
            if (_started)
            {
                throw new InvalidOperationException();
            }

            _stopped = false;
            _started = true;
            _startedAt = DateTimeOffset.UtcNow;
        }

        public void Stop()
        {
            if (_stopped)
            {
                throw new InvalidOperationException();
            }

            _stopped = true;
            _stoppedAt = DateTimeOffset.UtcNow;
        }

        public bool HasStarted => _started;
        public bool HasStopped => _stopped;
        
        public DateTimeOffset StartedAt => _started ? _startedAt : throw new InvalidOperationException();
        public DateTimeOffset StoppedAt => _stopped ? _stoppedAt : throw new InvalidOperationException();

        public TimeSpan Uptime
        {
            get
            {
                if (_started)
                {
                    return (_stopped ? _stoppedAt : DateTimeOffset.UtcNow) - _startedAt;
                }
                
                throw new InvalidOperationException();
            }
        }
    }
}
