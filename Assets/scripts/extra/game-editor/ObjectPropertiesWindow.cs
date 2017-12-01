using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPropertiesWindow : MonoBehaviour 
{
  public GameObject WindowHolder;

  public BaseObjectProperties BasePropertiesScript;
  public ExitZoneProperties ExitZonePropertiesScript;

  public void SelectObject(WorldObjectBase obj)
  {
    HideWindows();

    if (obj is ExitZoneObject)
    {
      ExitZonePropertiesScript.Init(obj);
      ExitZonePropertiesScript.gameObject.SetActive(true);
    }
    else
    {
      BasePropertiesScript.Init(obj);
      BasePropertiesScript.gameObject.SetActive(true);
    }

    WindowHolder.SetActive(true);
  }

  public void DeselectObject()
  {
    WindowHolder.SetActive(false);
    HideWindows();
  }

  void HideWindows()
  {
    BasePropertiesScript.gameObject.SetActive(false);
    ExitZonePropertiesScript.gameObject.SetActive(false);
  }
}
