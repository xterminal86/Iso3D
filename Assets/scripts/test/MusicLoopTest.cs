using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLoopTest : MonoBehaviour 
{
  public AudioSource MusicTrack;

  double _sampleRate = 0.0f;
  double _loopStart = 0.0f;
	void Start () 
  {
    _sampleRate = AudioSettings.outputSampleRate;
    _loopStart = 326663 / _sampleRate;
    GameObject loopSegment = new GameObject();
    AudioSource loop = loopSegment.AddComponent<AudioSource>();
    loop.playOnAwake = false;
    loop.clip = MusicTrack.clip;
    loop.timeSamples = 326663;
    loop.loop = true;
    loop.PlayScheduled(AudioSettings.dspTime + MusicTrack.clip.length);
    MusicTrack.Play();
    //Debug.Log(_loopStart);
    //MusicTrack.PlayScheduled(_loopStart);
    //MusicTrack.SetScheduledStartTime(_loopStart);
	}
}
