using System;
using GTA;
using SmokeModGTAIV.Animation;
using SmokeModGTAIV.Config;
using SmokeModGTAIV.Effects;
using SmokeModGTAIV.Input;
using SmokeModGTAIV.Prop;
using SmokeModGTAIV.State;

namespace SmokeModGTAIV.Core
{
    internal class SmokingController
    {
        private const int ExtinguishDurationMs = 1500;

        private readonly SmokeConfig      _config;
        private readonly ISmokingAnimator _animator;
        private readonly ISmokeProp       _prop;
        private readonly BurnTimer        _burnTimer;
        private readonly ISmokingEffect[] _effects;

        private SmokingState _state = SmokingState.Idle;
        private DateTime     _stateEnterTime;
        private DateTime     _lastPuffTime = DateTime.MinValue;

        public SmokingState CurrentState => _state;

        public SmokingController(
            SmokeConfig config,
            ISmokingAnimator animator,
            ISmokeProp prop,
            BurnTimer burnTimer,
            ISmokingEffect[] effects)
        {
            _config    = config;
            _animator  = animator;
            _prop      = prop;
            _burnTimer = burnTimer;
            _effects   = effects;
        }

        public void HandleInput(SmokingInput input, Ped player)
        {
            if (input == SmokingInput.None) return;

            if (input == SmokingInput.ActionKey && _state == SmokingState.Idle)
            {
                TransitionTo(SmokingState.LoadingAnims, player);
                return;
            }

            if (input == SmokingInput.ActionKey && _state == SmokingState.Smoking)
            {
                ExecutePuff(player);
                return;
            }

            if (input == SmokingInput.ExtinguishKey && _state == SmokingState.Smoking)
                TransitionTo(SmokingState.Extinguishing, player);
        }

        public void Tick(Ped player)
        {
            _burnTimer.Tick();

            foreach (var effect in _effects)
                effect.OnTick(player);

            if (_state != SmokingState.Idle && !player.isAliveAndWell)
            {
                ForceCleanup(player);
                return;
            }

            if (_state == SmokingState.Smoking && _burnTimer.IsExpired)
            {
                TransitionTo(SmokingState.Extinguishing, player);
                return;
            }

            switch (_state)
            {
                case SmokingState.LoadingAnims:  TickLoadingAnims(player); break;
                case SmokingState.Lighting:      TickLighting(player);     break;
                case SmokingState.Smoking:       TickSmoking(player);      break;
                case SmokingState.Extinguishing: TickExtinguishing(player); break;
            }
        }

        private void TickLoadingAnims(Ped player)
        {
            _animator.RequestAnims();
            if (_animator.AreAnimsLoaded())
                TransitionTo(SmokingState.Lighting, player);
        }

        private void TickLighting(Ped player)
        {
            if (ElapsedMs() >= _config.LightingDurationMs)
                TransitionTo(SmokingState.Smoking, player);
        }

        private void TickSmoking(Ped player)
        {
            if ((DateTime.Now - _lastPuffTime).TotalMilliseconds >= _config.AutoPuffIntervalMs)
                ExecutePuff(player);
        }

        private void TickExtinguishing(Ped player)
        {
            if (ElapsedMs() >= ExtinguishDurationMs)
                TransitionTo(SmokingState.Idle, player);
        }

        private void ExecutePuff(Ped player)
        {
            _animator.PlayPuff(player);
            _lastPuffTime = DateTime.Now;

            foreach (var effect in _effects)
                effect.OnPuff(player);
        }

        private void TransitionTo(SmokingState next, Ped player)
        {
            _state          = next;
            _stateEnterTime = DateTime.Now;

            switch (next)
            {
                case SmokingState.LoadingAnims:
                    ShowHud("Encendiendo...", 1000);
                    break;

                case SmokingState.Lighting:
                    _animator.PlayLighting(player);
                    break;

                case SmokingState.Smoking:
                    _prop.Attach(player);
                    _prop.StartSmoke();
                    _burnTimer.Start(_config.BurnDurationMs);
                    _lastPuffTime = DateTime.Now;
                    ShowHud($"~g~Encendido  ~w~{_config.KeyStartSmoking} calada | {_config.KeyExtinguish} apagar", 3000);
                    break;

                case SmokingState.Extinguishing:
                    _animator.PlayExtinguish(player);
                    _prop.Detach();
                    _burnTimer.Stop();
                    foreach (var effect in _effects) effect.OnStop(player);
                    ShowHud("Apagando...", 1200);
                    break;

                case SmokingState.Idle:
                    ShowHud("Cigarrillo apagado", 2000);
                    break;
            }
        }

        private void ForceCleanup(Ped player)
        {
            _prop.Detach();
            _burnTimer.Stop();
            foreach (var effect in _effects) effect.OnStop(player);
            _state = SmokingState.Idle;
        }

        private double ElapsedMs() => (DateTime.Now - _stateEnterTime).TotalMilliseconds;

        private void ShowHud(string message, int durationMs)
        {
            if (_config.ShowHud) Game.DisplayText(message, durationMs);
        }
    }
}
