using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using TMPro;


public class SoundLoader : MonoBehaviour
{
    public string FileType = "";
    public int ChunkSize;
    public int SubChunkSize;
    public int Format;
    public int Channels;
    public int SampleRate;
    public int ByteRate;
    public int BlockAlign;
    public int BytesPerSample;
    public int SubChunk2Size;
    public byte[] AudioData;
    public float[] SampleData;

	/// <summary>
    /// Loads the wav file at the given path (checking that it is actually a wav file)
    /// </summary>
    /// <param name="path">The path to load the wav file from</param>
    /// <returns>A float[] containing the sample data, ranging from -1 to 1</returns>
    public float[] ImportAudio(string path)
	{
        byte[] fileBytes;

        if (!File.Exists(path))
        {//the user might have just specified the fir part, so we need to also check name + ".wav"
            if (!File.Exists(path + ".wav"))
            {
                //The file doesnt exist, give an error
                GameObject.Find("ErrorBox").GetComponent<TextMeshProUGUI>().text = "The specified file cannot be found, please make sure that it exists";
                return null;
            }
            else
            {
                fileBytes = File.ReadAllBytes(path + ".wav");
            }            
        }
        else
        {
           fileBytes = File.ReadAllBytes(path);
        }
        

        FileType = ((char)fileBytes[0]).ToString() + ((char)fileBytes[1]).ToString() + ((char)fileBytes[2]).ToString() + ((char)fileBytes[3]).ToString();

        if (FileType != "RIFF")
        {
            //The file isnt wav, give an error
            GameObject.Find("ErrorBox").GetComponent<TextMeshProUGUI>().text = "The specified file is not a wav file, please check it, it may be corrupted, or simply the wrong file";
            return null;
        }

        //Offsets taken from the wav specification, an example of which can be found here: http://soundfile.sapp.org/doc/WaveFormat/
        ChunkSize = LittleEndianToDecimal(GetValuesAtOffset(fileBytes, "0x4", "0x8"));
        SubChunkSize = LittleEndianToDecimal(GetValuesAtOffset(fileBytes, "0x10", "0x14"));
        Format = LittleEndianToDecimal(GetValuesAtOffset(fileBytes, "0x14", "0x16"));
        Channels = LittleEndianToDecimal(GetValuesAtOffset(fileBytes, "0x16", "0x18"));
        SampleRate = LittleEndianToDecimal(GetValuesAtOffset(fileBytes, "0x18", "0x1C"));
        ByteRate = LittleEndianToDecimal(GetValuesAtOffset(fileBytes, "0x1C", "0x20"));
        BlockAlign = LittleEndianToDecimal(GetValuesAtOffset(fileBytes, "0x20", "0x22"));

        BytesPerSample = LittleEndianToDecimal(GetValuesAtOffset(fileBytes, "0x22", "0x24")) / 8;

        SubChunk2Size = LittleEndianToDecimal(GetValuesAtOffset(fileBytes, "0x28", "0x2C"));

        AudioData = fileBytes.Skip(Convert.ToInt32("0x2C", 16)).Take(fileBytes.Length - Convert.ToInt32("0x2C", 16)).ToArray();
        List<float> sampleFloats = new List<float>();

        //converts every sample from the little-endian hex format to a float betwen -1 and 1
        for (int i = 0; i < AudioData.Length; i += BytesPerSample)
        {
            sampleFloats.Add(TwosComplementBinaryToDecimal(Convert.ToString(LittleEndianToDecimal(AudioData.Skip(i).Take(BytesPerSample).ToArray()), 2)) / 32768f);
        }
        SampleData = sampleFloats.ToArray();

        return SampleData;

    }

    /// <summary>
    /// Converts a little endian hex number ("3F 01") to its corresponding decimal number (319)
    /// </summary>
    /// <param name="data">The bytes that represent the hex number</param>
    /// <returns>the corresponding decimal number</returns>
    public int LittleEndianToDecimal(byte[] data)
    {
        Array.Reverse(data, 0, data.Length);
        return Convert.ToInt32("0x" + String.Join(string.Empty, Array.ConvertAll(data, x => x.ToString("X2"))), 16);
    }

    /// <summary>
    /// Returns the elements of an array specified with hex strings
    /// </summary>
    /// <param name="data">The array to take the data from</param>
    /// <param name="StartOffset">The position to start taking data</param>
    /// <param name="EndOffset">The position to stop taking data</param>
    /// <returns>An array of data</returns>
    public byte[] GetValuesAtOffset(byte[] data, string StartOffset, string EndOffset)
    {
        int startOffsetInt = Convert.ToInt32(StartOffset, 16);
        int endOffsetInt = Convert.ToInt32(EndOffset, 16);

        return data.Skip(startOffsetInt).Take(endOffsetInt - startOffsetInt).ToArray();
    }

    /// <summary>
    /// Takes a two's complement encoded binary string ("10010101"), pads it to 16 bits and converts it back to an int (149)
    /// </summary>
    /// <param name="binary">The string of binary to be converted</param>
    /// <returns>The decimal representation of the binary</returns>
    public int TwosComplementBinaryToDecimal(string binary)
    {
        binary = binary.PadLeft(16, '0');
        if (binary[0] == '1')//The number is negative
        {
            char[] binaryArr = binary.ToCharArray();
            binaryArr[0] = '0';
            binary = new string(binaryArr);
            return - 32768 + Convert.ToInt32(binary, 2); //simple way to convert any binary out of being two's complement

        }
        else
        {
            return Convert.ToInt32(binary, 2);
        }        
    }
}
