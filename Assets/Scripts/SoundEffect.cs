using UnityEngine;
using System.Linq;

/// <summary>
/// Holds all the types of effects currently implemented
/// </summary>
public enum Effect {Riser, Double, Halve, RingMod};

/// <summary>
/// Responsible for applying sound effects to a given sound
/// </summary>
public class SoundEffect : MonoBehaviour
{
    /// <summary>
    /// Adds a rising volume effect to the audio, by creating a multiplier that increases over time
    /// </summary>
    /// <param name="data">The sound to be affected</param>
    /// <returns>The affected sound</returns>
    public static float[] Riser(float[] data)
    {
        int SampleLength = data.Length;
        for (int i = 0; i < SampleLength; i++)
        {
            data[i] /= SampleLength;
            data[i] *= i;
        }

        return data;
    }

    /// <summary>
    /// Doubles the frequency of the sound, by only returning every even-numbered sample
    /// </summary>
    /// <param name="data">The sound to be affected</param>
    /// <returns>The affected sound</returns>
    public static float[] Double(float[] data)
    {
        data = data.Where((source, index) => (index / 2) * 2 == index).ToArray();//This is LINQ magic: https://docs.microsoft.com/en-us/dotnet/csharp/tutorials/working-with-linq
        return data;
    }

    /// <summary>
    /// Halves the frequency of the sound, by duplicating every sample
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
    /// https://en.wikipedia.org/wiki/Ring_modulation
    /// </summary>
    /// <param name="data">The sound to be affected</param>
    /// <returns>The affected sound</returns>
    public static float[] RingMod (float[] data)
    {
        float[] ret = new float[data.Length];
        float modFreq = 300;//This is the modulation frequency. I would have liked for this to be changeable by the user, but I wasnt able to =(

        for (int i = 0; i < data.Length; i++)
        {            
            ret[i] = data[i] * (Mathf.Sin(i * modFreq / 44100) > 0 ? 1 : 0);
        }

        return ret;
    }

}
