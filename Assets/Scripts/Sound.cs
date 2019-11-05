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

    public void RefreshAudio()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.Stop();

        AudioClip NewAudio = AudioClip.Create("GeneratedWave", (int)(samplerate * length), 1, samplerate, false);
        NewAudio.SetData(GenerateData((int)length * samplerate), 0);

        source.clip = NewAudio;
        source.loop = Loop;

        source.Play();
        Debug.Log("Refreshed Audio!");
    }

    void Start()
    {
        RefreshAudio();
    }

    float[] GenerateData(int length)
    {
        float[] data = new float[length];

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
                int sawLength = (int)(samplerate / frequency);
                int sawPosition = 0;

                while (count < data.Length)
                {
                    data[count] = (float)sawPosition / sawLength;
                    position++;
                    count++;
                    sawPosition = position % sawLength;
                    
                }
                break;

            case Waveform.Square:
                int squareLength = (int)(samplerate / frequency);
                while (count < data.Length)
                {
                    data[count] = position % squareLength > squareLength / 2 ? 0 : 1;
                    position++;
                    count++;
                }
                break;

            default:
                break;
        }

        return data;
        
    }

    void OnAudioSetPosition(int newPosition)
    {
        position = newPosition;
    }
}
