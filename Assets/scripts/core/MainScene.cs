using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour 
{
  public GameObject PortraitGui;
  public GameObject LowerPanel;
  public GameObject UnityBar;

  public void LowerPanelMouseEnter()
  {
    PortraitGui.SetActive(true);
    LowerPanel.SetActive(true);
    UnityBar.SetActive(true);
  }

  public void LowerPanelMouseExit()
  {
    PortraitGui.SetActive(false);
    LowerPanel.SetActive(false);
    UnityBar.SetActive(false);
  }
}
