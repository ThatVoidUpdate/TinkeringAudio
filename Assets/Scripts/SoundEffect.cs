using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Holds all the types of effects currently implemented
/// </summary>
public enum Effect {Riser, Double, Halve, RingMod};
public class SoundEffect : MonoBehaviour
{
    /// <summary>
    /// Adds a rising volume effect to the audio
    /// </summary>
    /// <param name="data">The sound to be affected</param>
    /// <returns>The affected sound</returns>
    public static float[] Riser(float[] data)
    {
        int SampleLength = data.Length;
        for (int i = 0; i < SampleLength; i++)
        {
            data[i] = data[i] / SampleLength;
            data[i] = data[i] * i;
        }

        return data;
    }

    /// <summary>
    /// Doubles the frequency of the sound
    /// </summary>
    /// <param name="data">The sound to be affected</param>
    /// <returns>The affected sound</returns>
    public static float[] Double(float[] data)
    {
        data = data.Where((source, index) => (index / 2) * 2 == index).ToArray();
        return data;
    }

    /// <summary>
    /// Halves the frequency of the sound
    /// </summary>
    /// <param name="data">The sound to be affected</param>
    /// <returns>The affected sound</returns>
    public static float[] Halve(float[] data)
    {
        float[] ret = new float[data.Length * 2];

        for (int i = 0; i < data.Length * 2; i += 2)
        {
            ret[i] = data[i / 2];
            ret[i + 1] = data[i / 2];
        }

        return ret;
    }

    /// <summary>
    /// Adds a ring modulation effect with 300hz as the modulation frequency
    /// </summary>
    /// <param name="data">The sound to be affected</param>
    /// <returns>The affected sound</returns>
    public static float[] RingMod (float[] data)
    {
        float[] ret = new float[data.Length];
        float modFreq = 300;

        for (int i = 0; i < data.Length; i++)
        {            
            ret[i] = data[i] * (Mathf.Sin(i * modFreq / 44100) > 0 ? 1 : 0);
        }

        return ret;
    }

}
