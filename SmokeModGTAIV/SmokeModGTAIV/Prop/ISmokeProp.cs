using GTA;

namespace SmokeModGTAIV.Prop
{
    internal interface ISmokeProp
    {
        bool IsAlive { get; }
        void Attach(Ped player);
        void Detach();
        void StartSmoke();
        void StopSmoke();
    }
}
