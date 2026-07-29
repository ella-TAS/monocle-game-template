using Microsoft.Xna.Framework.Audio;
using Monocle;

namespace Gamespace;

public static class SFX {
    // public static SoundEffect Countdown;
    // public static SoundEffect Music;

    private static readonly List<SoundEffectInstance> loopingSounds = new();

    public static void Load() {

    }

    private static SoundEffect loadSFX(string name) {
        return Engine.Instance.Content.Load<SoundEffect>("Audio/SFX/" + name);
    }

    private static SoundEffect loadLooping(string name) {
        return Engine.Instance.Content.Load<SoundEffect>("Audio/Looping/" + name);
    }

    public static SoundEffectInstance Play(SoundEffect sfx, float volume) {
        SoundEffectInstance sfxInstance = sfx.CreateInstance();
        sfxInstance.Volume = volume * SliderToVolume(SaveData.Instance.SFXVolume);
        sfxInstance.Play();
        return sfxInstance;
    }

    public static SoundEffectInstance PlayLooping(SoundEffect sfx, float volume) {
        SoundEffectInstance sfxInstance = sfx.CreateInstance();
        sfxInstance.Volume = volume * SliderToVolume(SaveData.Instance.MusicVolume);
        sfxInstance.IsLooped = true;
        loopingSounds.Add(sfxInstance);
        sfxInstance.Play();
        return sfxInstance;
    }

    public static void UpdateVolume(float volume) {
        foreach (SoundEffectInstance sfxInstance in loopingSounds) {
            sfxInstance.Volume = volume * SliderToVolume(SaveData.Instance.MusicVolume);
        }
    }

    /// <summary>
    /// Converts a linear scale to something that sounds linear
    /// </summary>
    public static float SliderToVolume(float volume) {
        // {10^{2x}-1}/{99}
        return (float) (Math.Pow(10, 2 * volume) - 1) / 99f;
    }
}
