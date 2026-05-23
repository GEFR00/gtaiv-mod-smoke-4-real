using GTA;
using GTA.Native;

namespace SmokeModGTAIV.Prop
{
    internal class CigaretteProp : ISmokeProp
    {
        private const string ModelName    = "bm_char_fag_f";
        private const string ParticleName = "ambient_cig_smoke";
        private const int    HandBoneId   = 1232;

        private const float AttachOffsetX = 0.015f;
        private const float AttachOffsetY = -0.005f;
        private const float AttachOffsetZ = -0.021f;

        private const float SmokeOffsetX = 0.125f;
        private const float SmokeOffsetY = -0.02f;
        private const float SmokeOffsetZ =  0.01f;
        private const float SmokeScale   = 1.1f;

        private GTA.Object _prop;

        public bool IsAlive =>
            _prop != null && Function.Call<bool>("DOES_OBJECT_EXIST", _prop);

        public void Attach(Ped player)
        {
            _prop = World.CreateObject(ModelName, player.Position);

            Function.Call("ATTACH_OBJECT_TO_PED",
                _prop, player,
                HandBoneId,
                AttachOffsetX, AttachOffsetY, AttachOffsetZ,
                0.0f, 0.0f, 0.0f,
                0);
        }

        public void Detach()
        {
            if (!IsAlive) { _prop = null; return; }
            _prop.Delete();
            _prop = null;
        }

        public void StartSmoke()
        {
            if (!IsAlive) return;
            Function.Call("START_PTFX_ON_OBJ",
                ParticleName,
                _prop,
                SmokeOffsetX, SmokeOffsetY, SmokeOffsetZ,
                0.0f, 0.0f, 0.0f,
                SmokeScale);
        }

        public void StopSmoke()
        {
            // Particles are tied to the prop; Detach() handles full cleanup.
        }
    }
}
