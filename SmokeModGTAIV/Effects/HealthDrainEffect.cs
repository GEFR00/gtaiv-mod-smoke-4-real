using GTA;
using SmokeModGTAIV.Config;

namespace SmokeModGTAIV.Effects
{
    internal class HealthDrainEffect : ISmokingEffect
    {
        private readonly SmokeConfig _config;

        public HealthDrainEffect(SmokeConfig config)
        {
            _config = config;
        }

        public void OnPuff(Ped player)
        {
            if (!_config.EnableHealthDrain) return;
            int next = player.Health - _config.HealthDrainPerPuff;
            player.Health = next < 1 ? 1 : next;
        }

        public void OnTick(Ped player) { }

        public void OnStop(Ped player) { }
    }
}
