using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HighlightableControl : MonoBehaviour 
{
  public CustomControlGroup ControlGroupRef;

  public AudioSource HighlightSound;

  public List<AudioSource> ClickSounds;

  public UnityEvent MethodToCall;

  [HideInInspector]
  public bool Selected = false;

  public virtual void ResetStatus()
  {
    Selected = false;
  }

  public virtual void Select() { }
}
