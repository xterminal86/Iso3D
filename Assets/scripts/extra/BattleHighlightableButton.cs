using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleHighlightableButton : HighlightableButton 
{
  public override void OnMouseDown(BaseEventData data)
  {
    if (!Enabled || !Highlighted || BattleController.Instance.IsPaused || Selected)
    {
      return;
    }

    if (ClickSounds.Count > 0)
    {
      int index = Random.Range(0, ClickSounds.Count);
      ClickSounds[index].Play();
    }

    HighlightedSprite.SetActive(false);
    SelectedSprite.SetActive(true);

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
