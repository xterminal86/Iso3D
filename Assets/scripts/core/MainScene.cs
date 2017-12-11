using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour 
{
  public GameObject PortraitGui;
  public GameObject UnityBar;

  public void MouseEnterHandler()
  {
    PortraitGui.SetActive(true);
    UnityBar.SetActive(true);
  }

  public void MouseExitHandler()
  {
    PortraitGui.SetActive(false);
    UnityBar.SetActive(false);
  }
}
