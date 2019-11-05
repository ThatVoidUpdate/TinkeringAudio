using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Waveform {Sine, Sawtooth, Square}


[RequireComponent(typeof(AudioSource))]
public class Sound : MonoBehaviour
{
    private int position = 0;
    public Waveform form = Waveform.Sine;
    public int samplerate = 44100;
    public float frequency = 440;
    public float length = 2;
    public bool Loop = false;
    private int count = 0;

    void Start()
    {
        AudioClip Audio = AudioClip.Create("MySinusoid", (int)(samplerate * length), 1, samplerate, true, OnAudioRead, OnAudioSetPosition);
        AudioSource source = GetComponent<AudioSource>();
        source.clip = Audio;
        source.loop = Loop;
        source.Play();
    }

    void OnAudioRead(float[] data)
    {
        
        switch (form)
        {
            case Waveform.Sine:
                while (count < data.Length)
                {
                    data[count] = Mathf.Sin(2 * Mathf.PI * frequency * position / samplerate);
                    position++;
                    count++;
                }
                break;

            case Waveform.Sawtooth:
                int sawLength = (int)(samplerate/frequency);
                Debug.Log(sawLength);
                int sawPosition = 0;

                while (count < data.Length)
                {
                    data[count] = (float)sawPosition / sawLength;
                    //Debug.Log("Position: " + sawPosition + ", length: " + sawLength + ", amplitude: " + (float)sawPosition / sawLength);
                    position++;
                    count++;
                    sawPosition = position % sawLength;
                    
                }
                break;

            case Waveform.Square:
                while (count < data.Length)
                {
                    data[count] = Mathf.Sin(2 * Mathf.PI * frequency * position / samplerate);
                    position++;
                    count++;
                }
                break;

            default:
                break;
        }
        
    }

    void OnAudioSetPosition(int newPosition)
    {
        position = newPosition;
    }
}
