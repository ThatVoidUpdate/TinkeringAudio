using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SoundLoader))]
public class SoundPlayer : MonoBehaviour
{
    public Effect[] Effects;

    public TMP_InputField input;

    private float[] BaseAudio;
    private float[] EffectAudio;

    /// <summary>
    /// Applies the currently selected effects to the sound, assigns it to the audio source, and plays it
    /// </summary>
    public void PlayAudio()
    {
		AudioSource source = GetComponent<AudioSource>();

        if (source.isPlaying)
        {
            source.Stop();
        }
        else
        {
            ApplyEffects(); 
            AudioClip NewAudio = AudioClip.Create("GeneratedWave", EffectAudio.Length, 1, 44100, false);
            NewAudio.SetData(EffectAudio, 0);
            source.clip = NewAudio;

            source.Play();
        }
	}

    /// <summary>
    /// Uses the SoundLoader script to load in a wav file
    /// </summary>
    public void LoadAudio()
    {
        BaseAudio = GetComponent<SoundLoader>().ImportAudio("Assets\\" + input.text);
        if (BaseAudio != null)
        {
            GameObject.Find("PlayButton").GetComponent<Button>().interactable = true;
            GameObject.Find("ErrorBox").GetComponent<TextMeshProUGUI>().text = "";
        }
        else
        {
            GameObject.Find("PlayButton").GetComponent<Button>().interactable = false;
        }
    }

    /// <summary>
    /// Applies all the selected audio effects in the effect chain
    /// </summary>
    public void ApplyEffects()
    {
        EffectAudio = new float[BaseAudio.Length];
        Array.Copy(BaseAudio, 0, EffectAudio, 0, BaseAudio.Length);
        foreach (Effect effect in Effects)
        {
            switch (effect)
            {
                case Effect.Riser:
                    EffectAudio = SoundEffect.Riser(EffectAudio);
                    break;
                case Effect.Double:
                    EffectAudio = SoundEffect.Double(EffectAudio);
                    break;
                case Effect.Halve:
                    EffectAudio = SoundEffect.Halve(EffectAudio);
                    break;
                case Effect.RingMod:
                    EffectAudio = SoundEffect.RingMod(EffectAudio);
                    break;
            }
        }
    }

    /// <summary>
    /// Sets the first audio effect to be applied to the audio
    /// </summary>
    /// <param name="effect">The index into Effect to be applied</param>
    public void SetFirstEffect(int effect)
    {
        Effects[0] = (Effect)effect;
    }

    /// <summary>
    /// Sets the second audio effect to be applied to the audio
    /// </summary>
    /// <param name="effect">The index into Effect to be applied</param>
    public void SetSecondEffect(int effect)
    {
        Effects[1] = (Effect)effect;
    }
}
