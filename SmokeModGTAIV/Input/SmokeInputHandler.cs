using SmokeModGTAIV.Config;

namespace SmokeModGTAIV.Input
{
    internal class SmokeInputHandler
    {
        private readonly SmokeConfig _config;

        public SmokeInputHandler(SmokeConfig config)
        {
            _config = config;
        }

        public SmokingInput Translate(GTA.KeyEventArgs e)
        {
            if (e.Key == _config.KeyStartSmoking) return SmokingInput.ActionKey;
            if (e.Key == _config.KeyExtinguish)   return SmokingInput.ExtinguishKey;
            return SmokingInput.None;
        }
    }
}
