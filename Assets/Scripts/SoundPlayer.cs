using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SoundLoader))]

///<summary>
///Responsible for playing audio
///</summary>
public class SoundPlayer : MonoBehaviour
{
    //Holds all the effects to be applied, in order
    public Effect[] Effects;

    //The input box for the filename
    public TMP_InputField filename;

    //Holds the normal and effected audio
    private float[] BaseAudio;
    private float[] effectAudio;

    /// <summary>
    /// Applies the currently selected effects to the sound, assigns it to the audio source, and plays it
    /// </summary>
    public void PlayAudio()
    {
		AudioSource audioSource = GetComponent<AudioSource>();

        //A toggle to stop the sound if it is playing, or generate a new sound and play it if it is stopped
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        else
        {
            ApplyEffects(); 
            AudioClip NewAudio = AudioClip.Create("GeneratedWave", effectAudio.Length, 1, 44100, false);
            NewAudio.SetData(effectAudio, 0);
            audioSource.clip = NewAudio;

            audioSource.Play();
        }
	}

    /// <summary>
    /// Uses the SoundLoader script to load in a wav file
    /// </summary>
    public void LoadAudio()
    {
        BaseAudio = GetComponent<SoundLoader>().ImportAudio("Assets\\" + filename.text);
        if (BaseAudio != null) //We got data back, so enable the play button, and clear the error dialog box
        {
            GameObject.Find("PlayButton").GetComponent<Button>().interactable = true;
            GameObject.Find("ErrorBox").GetComponent<TextMeshProUGUI>().text = "";
        }
        else //If we didnt get any data back, there must have been an error, so disable the play button. Errors are printed inside the error box by ImportAudio
        {
            GameObject.Find("PlayButton").GetComponent<Button>().interactable = false;
        }
    }

    /// <summary>
    /// Applies all the selected audio effects in the effect chain
    /// </summary>
    public void ApplyEffects()
    {
        effectAudio = new float[BaseAudio.Length];
        Array.Copy(BaseAudio, 0, effectAudio, 0, BaseAudio.Length);
        foreach (Effect effect in Effects)
        {//Apply all the effects
            switch (effect)
            {
                case Effect.Riser:
                    effectAudio = SoundEffect.Riser(effectAudio);
                    break;
                case Effect.Double:
                    effectAudio = SoundEffect.Double(effectAudio);
                    break;
                case Effect.Halve:
                    effectAudio = SoundEffect.Halve(effectAudio);
                    break;
                case Effect.RingMod:
                    effectAudio = SoundEffect.RingMod(effectAudio);
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

    //Both functions above have to exist how they are, as Unity dropdown menus can only give 1 argument of type int
}
