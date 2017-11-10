using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HighlightableText : HighlightableControl 
{  
  public Text NormalText;
  public Text HighlightedText;

  public void OnMouseEnter()
  {
    if (Selected)
    {
      return;
    }

    HighlightSound.Play();

    NormalText.gameObject.SetActive(false);
    HighlightedText.gameObject.SetActive(true);
  }

  public void OnMouseExit()
  {
    if (Selected)
    {
      return;
    }

    HighlightedText.gameObject.SetActive(false);
    NormalText.gameObject.SetActive(true);
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

    NormalText.gameObject.SetActive(true);
    HighlightedText.gameObject.SetActive(false);
  }

  public override void Select()
  {
    Selected = true;

    NormalText.gameObject.SetActive(false);
    HighlightedText.gameObject.SetActive(true);

    if (MethodToCall != null)
    {
      MethodToCall.Invoke();
    }     
  }
}
