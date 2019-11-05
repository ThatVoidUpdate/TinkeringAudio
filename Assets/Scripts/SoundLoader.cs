using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLoader : MonoBehaviour
{
	public float[] AudioData;
	
    public void ImportAudio(string path)
	{
		AudioData = new float[44100];
		for(int i = 0; i < 44100; i++)
		{
			AudioData[i] = 1;
		}
	}
}
