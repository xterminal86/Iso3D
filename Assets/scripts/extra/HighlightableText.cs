﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HighlightableText : HighlightableControl 
{  
  public Text NormalText;
  public Text HighlightedText;
  public Text DisabledText;

  void Start()
  {
    if (!Enabled)
    {
      NormalText.gameObject.SetActive(false);
      HighlightedText.gameObject.SetActive(false);
      DisabledText.gameObject.SetActive(true);
    }
    else
    {
      NormalText.gameObject.SetActive(true);
      HighlightedText.gameObject.SetActive(false);
      DisabledText.gameObject.SetActive(false);
    }
  }

  public void OnMouseEnter()
  {
    if (Selected || !Enabled)
    {
      return;
    }

    HighlightSound.Play();

    NormalText.gameObject.SetActive(false);
    HighlightedText.gameObject.SetActive(true);
  }

  public void OnMouseExit()
  {
    if (Selected || !Enabled)
    {
      return;
    }

    HighlightedText.gameObject.SetActive(false);
    NormalText.gameObject.SetActive(true);
  }

  public void OnMouseDown()
  {
    if (!Enabled)
    {
      return;
    }

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
        MethodToCall.Invoke(this);
      }

      if (MethodToCall0 != null)
      {
        Selected = true;
        MethodToCall0.Invoke();
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
    DisabledText.gameObject.SetActive(false);

    if (MethodToCall != null)
    {
      MethodToCall.Invoke(this);
    }

    if (MethodToCall0 != null)
    {
      MethodToCall0.Invoke();
    }
  }

  public override void SetStatus(bool isEnabled)
  {
    Enabled = isEnabled;

    if (!Enabled)
    {
      NormalText.gameObject.SetActive(false);
      HighlightedText.gameObject.SetActive(false);
      DisabledText.gameObject.SetActive(true);
    }
    else
    {
      NormalText.gameObject.SetActive(true);
      HighlightedText.gameObject.SetActive(false);
      DisabledText.gameObject.SetActive(false);
    }
  }
}
