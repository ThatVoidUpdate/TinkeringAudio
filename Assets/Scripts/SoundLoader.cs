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

	
    public float[] ImportAudio(string path)
	{
        if (!File.Exists(path))
        {
            //the file doesnt exist, give an error
            GameObject.Find("ErrorBox").GetComponent<TextMeshProUGUI>().text = "The specified file cannot be found, please make sure that it exists";
            return null;
        }
        byte[] fileBytes = File.ReadAllBytes(path);

        FileType = ((char)fileBytes[0]).ToString() + ((char)fileBytes[1]).ToString() + ((char)fileBytes[2]).ToString() + ((char)fileBytes[3]).ToString();

        if (FileType != "RIFF")
        {
            GameObject.Find("ErrorBox").GetComponent<TextMeshProUGUI>().text = "The specified file is not a wav file, please check it";
            return null;
        }


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

        for (int i = 0; i < AudioData.Length; i += BytesPerSample)
        {
            sampleFloats.Add(TwosComplementBinaryToDecimal(Convert.ToString(LittleEndianToDecimal(AudioData.Skip(i).Take(BytesPerSample).ToArray()), 2)) / 32768f);
        }
        SampleData = sampleFloats.ToArray();

        return SampleData;

    }

    public int LittleEndianToDecimal(byte[] data)
    {
        Array.Reverse(data, 0, data.Length);
        return Convert.ToInt32("0x" + String.Join(string.Empty, Array.ConvertAll(data, x => x.ToString("X2"))), 16);
    }

    public byte[] GetValuesAtOffset(byte[] data, string StartOffset, string EndOffset)
    {
        int startOffsetInt = Convert.ToInt32(StartOffset, 16);
        int endOffsetInt = Convert.ToInt32(EndOffset, 16);

        return data.Skip(startOffsetInt).Take(endOffsetInt - startOffsetInt).ToArray();
    }

    public int TwosComplementBinaryToDecimal(string binary)
    {
        binary = binary.PadLeft(16, '0');
        if (binary[0] == '1')//negative
        {
            char[] binaryArr = binary.ToCharArray();
            binaryArr[0] = '0';
            binary = new string(binaryArr);
            return - 32768 + Convert.ToInt32(binary, 2);

        }
        else
        {
            return Convert.ToInt32(binary, 2);
        }        
    }
}
