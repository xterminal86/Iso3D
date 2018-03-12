using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HighlightableButton : HighlightableControl 
{
  public GameObject NormalSprite;
  public GameObject HighlightedSprite;
  public GameObject SelectedSprite;

  public override void OnMouseEnter(BaseEventData data)
  {
    if (Selected)
    {
      return;
    }

    Highlighted = true;

    if (HighlightSound != null)
    {
      HighlightSound.Play();
    }

    NormalSprite.SetActive(false);
    HighlightedSprite.SetActive(true);
  }

  public override void OnMouseExit(BaseEventData data)
  {
    if (Selected)
    {
      return;
    }

    Highlighted = false;

    HighlightedSprite.SetActive(false);
    NormalSprite.SetActive(true);
  }

  public override void OnMouseDown(BaseEventData data)
  {
    ProcessEvent();
  }

  public override void OnMouseUp(BaseEventData data)
  {
    ProcessEvent();
  }

  void ProcessEvent()
  {
    if (!Enabled || !Highlighted)
    {
      return;
    }

    if (ClickSounds.Count > 0)
    {
      int index = Random.Range(0, ClickSounds.Count);
      ClickSounds[index].Play();
    }

    if (Selected)
    {      
      return;
    }

    if (ControlGroupRef != null)
    {
      ControlGroupRef.ResetControls();
      Selected = true;
      InvokeMethod();
    }
    else
    {
      ResetStatus();
      InvokeMethod();
    }
  }

  void InvokeMethod()
  {    
    if (MethodToCallInEditor != null)
    {
      MethodToCallInEditor.Invoke(this);
    }

    if (MethodToCall0 != null)
    {
      MethodToCall0.Invoke();
    }
  }

  public override void ResetStatus()
  {
    base.ResetStatus();

    NormalSprite.SetActive(true);
    HighlightedSprite.SetActive(false);

    if (SelectedSprite != null)
    {
      SelectedSprite.SetActive(false);
    }
  }

  public override void Select()
  {
    Selected = true;

    NormalSprite.SetActive(false);
    HighlightedSprite.SetActive(false);

    if (SelectedSprite != null)
    {
      SelectedSprite.SetActive(true);
    }

    if (MethodToCallInEditor != null)
    {
      MethodToCallInEditor.Invoke(this);
    }

    if (MethodToCall0 != null)
    {
      MethodToCall0.Invoke();
    }
  }
}
