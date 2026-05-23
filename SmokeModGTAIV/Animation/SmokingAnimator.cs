using GTA;
using GTA.Native;

namespace SmokeModGTAIV.Animation
{
    internal class SmokingAnimator : ISmokingAnimator
    {
        private const string SetLighting   = "amb@smoking_spliff";
        private const string SetPuff       = "amb@smk_scn_idles";
        private const string SetExtinguish = "amb@nightclub_ext";

        private const string ClipLighting   = "create_spliff";
        private const string ClipPuff       = "stand_smoke";
        private const string ClipExtinguish = "smoke_stub_out";

        public void RequestAnims()
        {
            Function.Call("REQUEST_ANIMS", SetLighting);
            Function.Call("REQUEST_ANIMS", SetPuff);
            Function.Call("REQUEST_ANIMS", SetExtinguish);
        }

        public bool AreAnimsLoaded()
        {
            return Function.Call<bool>("HAS_ANIM_SET_LOADED", SetLighting)
                && Function.Call<bool>("HAS_ANIM_SET_LOADED", SetPuff)
                && Function.Call<bool>("HAS_ANIM_SET_LOADED", SetExtinguish);
        }

        public void PlayLighting(Ped player)
        {
            Function.Call("TASK_PLAY_ANIM_UPPER_BODY",
                player, ClipLighting, SetLighting,
                2.0f, 0, 0, 0, 0, -2);
        }

        public void PlayPuff(Ped player)
        {
            Function.Call("TASK_PLAY_ANIM_SECONDARY_UPPER_BODY",
                player, ClipPuff, SetPuff,
                2.0f, 0, 0, 0, 0, 3000);
        }

        public void PlayExtinguish(Ped player)
        {
            Function.Call("TASK_PLAY_ANIM_UPPER_BODY",
                player, ClipExtinguish, SetExtinguish,
                2.0f, 0, 0, 0, 0, -2);
        }
    }
}
