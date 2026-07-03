using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace RhythmXT;

internal static class SoundEffects
{
    static SoundEffect _soundEffect_Click;

    internal static void LoadContent(ContentManager content)
    {
        SoundEffect.MasterVolume = 0.4f;
        _soundEffect_Click = content.Load<SoundEffect>("click");
    }

    internal static void Click()
    {
        _soundEffect_Click.Play();
    }
}