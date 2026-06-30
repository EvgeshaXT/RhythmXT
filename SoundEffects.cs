using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace RhythmXT;

static internal class SoundEffects
{
    static SoundEffect _soundEffect_Click;

    static internal void LoadContent(ContentManager content)
    {
        SoundEffect.MasterVolume = 0.4f;
        _soundEffect_Click = content.Load<SoundEffect>("click");
    }

    static internal void Click()
    {
        _soundEffect_Click.Play();
    }
}