using System.IO;
using System.Windows.Forms;

namespace SmokeModGTAIV.Config
{
    internal class SmokeConfig
    {
        public Keys KeyStartSmoking  { get; private set; } = Keys.F6;
        public Keys KeyExtinguish    { get; private set; } = Keys.F7;

        public int LightingDurationMs { get; private set; } = 2000;
        public int BurnDurationMs     { get; private set; } = 120000;
        public int AutoPuffIntervalMs { get; private set; } = 15000;

        public bool EnableHealthDrain  { get; private set; } = true;
        public int  HealthDrainPerPuff { get; private set; } = 2;

        public bool EnableStaminaDrain { get; private set; } = true;
        public int  StaminaDrainMs     { get; private set; } = 5000;

        public bool ShowHud { get; private set; } = true;

        public static SmokeConfig Default() => new SmokeConfig();

        public static SmokeConfig Load(string iniPath)
        {
            WriteDefaultIfMissing(iniPath);

            var parser = new IniParser();
            parser.Load(iniPath);

            return new SmokeConfig
            {
                KeyStartSmoking   = parser.GetKey("StartSmoking",      Keys.F6),
                KeyExtinguish     = parser.GetKey("Extinguish",        Keys.F7),
                LightingDurationMs = parser.GetInt("LightingDurationMs", 2000),
                BurnDurationMs     = parser.GetInt("BurnDurationMs",    120000),
                AutoPuffIntervalMs = parser.GetInt("AutoPuffIntervalMs", 15000),
                EnableHealthDrain  = parser.GetBool("EnableHealthDrain", true),
                HealthDrainPerPuff = parser.GetInt("HealthDrainPerPuff", 2),
                EnableStaminaDrain = parser.GetBool("EnableStaminaDrain", true),
                StaminaDrainMs     = parser.GetInt("StaminaDrainMs",    5000),
                ShowHud            = parser.GetBool("ShowHud",          true)
            };
        }

        private static void WriteDefaultIfMissing(string iniPath)
        {
            if (File.Exists(iniPath)) return;
            File.WriteAllText(iniPath, DefaultContent);
        }

        private const string DefaultContent =
@"; SmokeModGTAIV - Configuration
; Key names use System.Windows.Forms.Keys (e.g. F1-F12, A-Z, NumPad0...)

[Keys]
StartSmoking = F6
Extinguish   = F7

[Timing]
LightingDurationMs  = 2000
BurnDurationMs      = 120000
AutoPuffIntervalMs  = 15000

[Effects]
EnableHealthDrain   = true
HealthDrainPerPuff  = 2
EnableStaminaDrain  = true
StaminaDrainMs      = 5000

[Debug]
ShowHud = true
";
    }
}
