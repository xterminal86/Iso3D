using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomControlGroup : MonoBehaviour 
{
  public List<HighlightableControl> Controls;

  void Start()
  {
    if (Controls.Count != 0)
    {
      ResetControls();
      Controls[0].Select();
    }
  }

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
