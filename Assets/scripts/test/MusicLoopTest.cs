using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLoopTest : MonoBehaviour 
{
  public AudioSource MusicTrack;

  // battle1: loopstart = 326663
  // tavern1: loopStart = 18847
  // tavern2: loopStart = 96369

  int _loopStartSamples = 96369; // 18847; 

	void Start() 
  {
    // To create endless loop first we play original track until the end
    // and then play endlessly fragment of the original track that needs to be looped.
    // We should use PlayScheduled for it uses audio system time (dspTime) thus making 
    // all track switching/manipulation framerate independent.

    GameObject loopSegment = new GameObject("Loop Fragment");
    AudioSource loop = loopSegment.AddComponent<AudioSource>();
    loop.playOnAwake = false;
    float[] samples = new float[(MusicTrack.clip.samples - _loopStartSamples) * MusicTrack.clip.channels];
    MusicTrack.clip.GetData(samples, _loopStartSamples);
    loop.clip = AudioClip.Create(MusicTrack.clip.name + "-loop", MusicTrack.clip.samples - _loopStartSamples, MusicTrack.clip.channels, MusicTrack.clip.frequency, false);
    loop.clip.SetData(samples, 0);
    loop.loop = true;
    loop.PlayScheduled(AudioSettings.dspTime + MusicTrack.clip.length);
    MusicTrack.Play();
	}
}
