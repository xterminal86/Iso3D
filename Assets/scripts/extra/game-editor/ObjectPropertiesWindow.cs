using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPropertiesWindow : MonoBehaviour 
{
  public GameObject WindowHolder;

  public BaseObjectProperties BasePropertiesScript;
  public ExitZoneProperties ExitZonePropertiesScript;
  public RampObjectProperties RampObjectPropertiesScript;

  WorldObjectBase _selectedObject;

  public void SelectObject(WorldObjectBase obj)
  {
    HideWindows();

    if (obj is ExitZoneObject)
    {
      ExitZonePropertiesScript.Init(obj);
      ExitZonePropertiesScript.gameObject.SetActive(true);
    }
    else if (obj is RampWorldObject)
    {
      RampObjectPropertiesScript.Init(obj);
      RampObjectPropertiesScript.gameObject.SetActive(true);
    }
    else
    {
      BasePropertiesScript.Init(obj);
      BasePropertiesScript.gameObject.SetActive(true);
    }

    _selectedObject = obj;

    WindowHolder.SetActive(true);
  }

  public void DeselectObject()
  {
    if (_selectedObject != null)
    {
      _selectedObject.Deselect();
    }

    WindowHolder.SetActive(false);
    HideWindows();
  }

  void HideWindows()
  {
    BasePropertiesScript.gameObject.SetActive(false);
    ExitZonePropertiesScript.gameObject.SetActive(false);
    RampObjectPropertiesScript.gameObject.SetActive(false);
  }
}
