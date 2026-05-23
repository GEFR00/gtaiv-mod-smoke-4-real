using System;

namespace SmokeModGTAIV.Effects
{
    internal class BurnTimer
    {
        private DateTime _endTime;
        private bool _running;

        public bool IsExpired { get; private set; }

        public void Start(int durationMs)
        {
            _endTime  = DateTime.Now.AddMilliseconds(durationMs);
            _running  = true;
            IsExpired = false;
        }

        public void Stop()
        {
            _running  = false;
            IsExpired = false;
        }

        public void Tick()
        {
            if (!_running || IsExpired) return;
            if (DateTime.Now >= _endTime)
            {
                _running  = false;
                IsExpired = true;
            }
        }
    }
}
