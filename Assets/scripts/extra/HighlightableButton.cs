using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HighlightableButton : HighlightableControl 
{
  public GameObject NormalSprite;
  public GameObject HighlightedSprite;

  public void OnMouseEnter()
  {
    if (Selected)
    {
      return;
    }

    HighlightSound.Play();

    NormalSprite.gameObject.SetActive(false);
    HighlightedSprite.gameObject.SetActive(true);
  }

  public void OnMouseExit()
  {
    if (Selected)
    {
      return;
    }

    HighlightedSprite.gameObject.SetActive(false);
    NormalSprite.gameObject.SetActive(true);
  }

  public void OnMouseDown()
  {
    int index = Random.Range(0, ClickSounds.Count);

    ClickSounds[index].Play();

    if (Selected)
    {
      return;
    }

    if (ControlGroupRef != null)
    {
      ControlGroupRef.ResetControls();

      if (MethodToCall != null)
      {
        Selected = true;
        MethodToCall.Invoke();
      }
    }
  }

  public override void ResetStatus()
  {
    base.ResetStatus();

    NormalSprite.gameObject.SetActive(true);
    HighlightedSprite.gameObject.SetActive(false);
  }

  public override void Select()
  {
    Selected = true;

    NormalSprite.gameObject.SetActive(false);
    HighlightedSprite.gameObject.SetActive(true);

    if (MethodToCall != null)
    {
      MethodToCall.Invoke();
    }     
  }
}
