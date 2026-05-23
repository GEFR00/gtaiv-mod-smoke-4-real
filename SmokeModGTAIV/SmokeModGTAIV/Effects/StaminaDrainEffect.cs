using System;
using GTA;
using GTA.Native;
using SmokeModGTAIV.Config;

namespace SmokeModGTAIV.Effects
{
    internal class StaminaDrainEffect : ISmokingEffect
    {
        // SET_CHAR_MAX_SPEED: lower value = slower max movement
        private const float SpeedReduced = 1.2f;  // above walk, below sprint
        private const float SpeedNormal  = 100.0f; // effectively uncapped

        private readonly SmokeConfig _config;
        private DateTime _drainUntil = DateTime.MinValue;
        private bool _drainActive;

        public StaminaDrainEffect(SmokeConfig config)
        {
            _config = config;
        }

        public void OnPuff(Ped player)
        {
            if (!_config.EnableStaminaDrain) return;
            _drainUntil = DateTime.Now.AddMilliseconds(_config.StaminaDrainMs);
        }

        public void OnTick(Ped player)
        {
            if (!_config.EnableStaminaDrain) return;

            bool shouldBeActive = DateTime.Now < _drainUntil;

            if (shouldBeActive && !_drainActive)
            {
                _drainActive = true;
                Function.Call("SET_CHAR_MAX_SPEED", player, SpeedReduced);
            }
            else if (!shouldBeActive && _drainActive)
            {
                ResetSpeed(player);
            }
        }

        public void OnStop(Ped player)
        {
            if (_drainActive) ResetSpeed(player);
        }

        private void ResetSpeed(Ped player)
        {
            _drainActive = false;
            Function.Call("SET_CHAR_MAX_SPEED", player, SpeedNormal);
        }
    }
}
