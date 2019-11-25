using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SoundLoader))]
public class SoundPlayer : MonoBehaviour
{
    public Effect[] Effects;

    public string file;

    private float[] BaseAudio;
    private float[] EffectAudio;
    public void PlayAudio()
	{
		AudioSource source = GetComponent<AudioSource>();

        AudioClip NewAudio = AudioClip.Create("GeneratedWave", EffectAudio.Length, 1, 44100, false);
        NewAudio.SetData(EffectAudio, 0);
        source.clip = NewAudio;

        source.Play();
	}

    public void LoadAudio()
    {
        BaseAudio = GetComponent<SoundLoader>().ImportAudio(file);
    }

    public void ApplyEffects()
    {
        EffectAudio = BaseAudio;
        foreach (Effect effect in Effects)
        {
            switch (effect)
            {
                case Effect.Riser:
                    BaseAudio = SoundEffect.Riser(EffectAudio);
                    break;
                case Effect.Double:
                    BaseAudio = SoundEffect.Double(EffectAudio);
                    break;
                case Effect.Halve:
                    BaseAudio = SoundEffect.Halve(EffectAudio);
                    break;
                case Effect.RingMod:
                    BaseAudio = SoundEffect.RingMod(EffectAudio);
                    break;
            }
        }
    }
}
