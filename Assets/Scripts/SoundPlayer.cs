using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    public Effect[] Effects;
    public void PlayAudio(float[] data)
	{
		AudioSource source = GetComponent<AudioSource>();

        AudioClip NewAudio = AudioClip.Create("GeneratedWave", data.Length, 1, 44100, false);
        NewAudio.SetData(data, 0);
        source.clip = NewAudio;

        source.Play();
	}

    public void Start()
    {
        float[] BaseAudio = GetComponent<SoundLoader>().ImportAudio("Assets\\Chipr.wav");
        foreach (Effect effect in Effects)
        {
            switch (effect)
            {
                case Effect.Riser:
                    BaseAudio = SoundEffect.Riser(BaseAudio);
                    break;
                case Effect.Double:
                    BaseAudio = SoundEffect.Double(BaseAudio);
                    break;
                case Effect.Halve:
                    BaseAudio = SoundEffect.Halve(BaseAudio);
                    break;
            }
        }
        PlayAudio(BaseAudio);
    }
}
