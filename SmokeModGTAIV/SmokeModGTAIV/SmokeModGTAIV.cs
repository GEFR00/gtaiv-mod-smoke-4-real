using GTA;
using System;
using System.IO;
using SmokeModGTAIV.Animation;
using SmokeModGTAIV.Config;
using SmokeModGTAIV.Core;
using SmokeModGTAIV.Effects;
using SmokeModGTAIV.Input;
using SmokeModGTAIV.Prop;

public class SmokeScript : Script
{
    private readonly SmokingController _controller;
    private readonly SmokeInputHandler _inputHandler;

    public SmokeScript()
    {
        SmokeConfig config = LoadConfig();

        ISmokingAnimator  animator  = new SmokingAnimator();
        ISmokeProp        prop      = new CigaretteProp();
        BurnTimer         burnTimer = new BurnTimer();
        ISmokingEffect[]  effects   =
        {
            new HealthDrainEffect(config),
            new StaminaDrainEffect(config)
        };

        _controller   = new SmokingController(config, animator, prop, burnTimer, effects);
        _inputHandler = new SmokeInputHandler(config);

        this.KeyDown += OnKeyDown;
        this.Tick    += OnTick;
    }

    private void OnKeyDown(object sender, GTA.KeyEventArgs e)
    {
        SmokingInput input = _inputHandler.Translate(e);
        _controller.HandleInput(input, Game.LocalPlayer.Character);
    }

    private void OnTick(object sender, EventArgs e)
    {
        _controller.Tick(Game.LocalPlayer.Character);
    }

    private static SmokeConfig LoadConfig()
    {
        try
        {
            // AppDomain.CurrentDomain.BaseDirectory is the GTA IV game folder
            string iniPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "scripts",
                "SmokeModGTAIV.ini");
            return SmokeConfig.Load(iniPath);
        }
        catch
        {
            return SmokeConfig.Default();
        }
    }
}
