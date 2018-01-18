using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(EventTrigger))]
public class HighlightableControl : MonoBehaviour 
{
  public CustomControlGroup ControlGroupRef;

  public AudioSource HighlightSound;

  public List<AudioSource> ClickSounds;

  public MyUnityEvent MethodToCall = new MyUnityEvent();
  public UnityEvent MethodToCall0;

  [HideInInspector]
  public bool Selected = false;

  // Mouse down can be invoked before mouse enter and exit, so we should watch this manually
  [HideInInspector]
  public bool Highlighted = false;

  public bool Enabled = true;

  public virtual void ResetStatus()
  {
    Selected = false;
    Highlighted = false;
  }

  public virtual void Select() { }
  public virtual void SetStatus(bool isEnabled) { }
  public virtual void OnMouseDown(BaseEventData data) { }
  public virtual void OnMouseEnter(BaseEventData data) { }
  public virtual void OnMouseExit(BaseEventData data) { }
}
