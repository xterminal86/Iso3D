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

  // This one is used in in-game editor to handle events when user clicks on text entry of objects in a list, for example.
  // Is set in GameEditor.cs, not visible in inspector.
  public MyUnityEvent MethodToCallInEditor = new MyUnityEvent();

  // Use this to set reference to a method via inspector (Unity-style)
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
  public virtual void OnMouseUp(BaseEventData data) { }
  public virtual void OnMouseEnter(BaseEventData data) { }
  public virtual void OnMouseExit(BaseEventData data) { }
}
