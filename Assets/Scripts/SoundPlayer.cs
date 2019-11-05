using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    public void PlayAudio(float[] data)
	{
		AudioSource source = GetComponent<AudioSource>();

        AudioClip NewAudio = AudioClip.Create("GeneratedWave", data.Length, 1, 44100, false);
        NewAudio.SetData(data, 0);
        source.clip = NewAudio;

        source.Play();
	}
}
