using System.Numerics;

namespace ArLib.Components
{
    public static class AFrameExtension
    {
        public static string ToA_Vector(this Vector3 vec) => $"{vec.X} {vec.Y} {vec.Z}";
        public static string ToA_Scale(this double scale) => $"{scale} {scale} {scale}";
        public static string ToA_Text(this bool value) => value ? "true" : "false";
        public static void ClassResolver(this Dictionary<string, object> attributes, string classText)
        {
            if (!string.IsNullOrWhiteSpace(classText)) attributes.Add("class", classText);
        }
        public static void AnimResolver(this Dictionary<string, object> attributes, Animation? anim)
        {
            if (anim == null) return;
            switch (anim.Type)
            {
                case Animation.AnimType.Simple:
                    attributes.Add("animation", anim.GetAttribute());
                    break;
                case Animation.AnimType.AnimationMixer:
                    attributes.Add("animation-mixer", anim.GetAttribute());
                    break;
            }
        }
    }
}
