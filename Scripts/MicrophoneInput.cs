using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MicrophoneInput : MonoBehaviour {
	public float sensitivity = 100;
	public float loudness = 1;
	private int minFreq;  
	private int maxFreq;  
	public AudioSource audio;

	void Start() {
		audio = GetComponent<AudioSource> ();
		audio.clip = Microphone.Start(null, true, 10, 44100);
		if (audio.clip == null) {
			Debug.LogError("Failed to start recording");

		}
		audio.loop = true; // Set the AudioClip to loop
		//audio.mute = true; // Mute the sound, we don't want the player to hear it
		while (!(Microphone.GetPosition(null) > 0)){

		} // Wait until the recording has started
		//Debug.LogError("mic position" + (Microphone.GetPosition(null)));
		Microphone.GetDeviceCaps (null, out minFreq, out maxFreq);
		if(minFreq == 0 && maxFreq == 0)  
		{  
			//...meaning 44100 Hz can be used as the recording sampling rate  
			maxFreq = 44100;  
		}  
		Debug.LogError("maxFreq" + maxFreq + "minFreq" + minFreq);
		//audio.Play(); // Play the audio source!
	}

	void Update(){
		loudness = GetAveragedVolume() * sensitivity;
		//Debug.LogError("Loudness" + loudness);
	}

	float GetAveragedVolume()
	{ 
		float[] data = new float[1024];
		float a = 0;
		audio.clip = Microphone.Start(null, true, 10, 44100);
		while (!(Microphone.GetPosition (null) > 0)) {
		}
		audio.GetOutputData(data,0);

		foreach(float s in data)
		{
			//Debug.LogError("output" + s);
			a += Mathf.Abs(s);
		}
		return a/1024;
	}
}