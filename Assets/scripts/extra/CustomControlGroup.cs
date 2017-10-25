using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomControlGroup : MonoBehaviour 
{
  public List<HighlightableButton> Controls;

  public void ResetControls()
  {
    foreach (var item in Controls)
    {
      if (item.Selected)
      {
        item.ResetStatus();
      }
    }
  }
}
