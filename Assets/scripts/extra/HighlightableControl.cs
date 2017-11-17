using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HighlightableControl : MonoBehaviour 
{
  public CustomControlGroup ControlGroupRef;

  public AudioSource HighlightSound;

  public List<AudioSource> ClickSounds;

  public MyUnityEvent MethodToCall = new MyUnityEvent();
  public UnityEvent MethodToCall0;

  [HideInInspector]
  public bool Selected = false;

  public bool Enabled = true;

  public virtual void ResetStatus()
  {
    Selected = false;
  }

  public virtual void Select() { }
  public virtual void SetStatus(bool isEnabled) { }
}
