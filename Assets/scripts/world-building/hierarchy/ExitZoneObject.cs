using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitZoneObject : WorldObjectBase 
{
  [HideInInspector]
  public SerializedExitZone ExitZoneToSave = new SerializedExitZone();

  void Start()
  {
    ExitZoneToSave.PrefabName = GlobalConstants.ExitZonePrefabName;
  }

  void OnTriggerEnter(Collider c)
  {
    Debug.Log("Loading " + ExitZoneToSave.LevelNameToLoad);
  }
}
