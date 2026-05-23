using GTA;

namespace SmokeModGTAIV.Effects
{
    internal interface ISmokingEffect
    {
        void OnPuff(Ped player);
        void OnTick(Ped player);
        void OnStop(Ped player);
    }
}
