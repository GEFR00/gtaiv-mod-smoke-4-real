using GTA;

namespace SmokeModGTAIV.Animation
{
    internal interface ISmokingAnimator
    {
        void RequestAnims();
        bool AreAnimsLoaded();
        void PlayLighting(Ped player);
        void PlayPuff(Ped player);
        void PlayExtinguish(Ped player);
    }
}
