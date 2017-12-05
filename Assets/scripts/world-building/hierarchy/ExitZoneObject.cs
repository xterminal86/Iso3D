using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitZoneObject : WorldObjectBase 
{  
  void Awake()
  {
    SerializedObject = new SerializedExitZone();

    PrefabName = GlobalConstants.ExitZonePrefabName;
  }

  public override void Init(SerializedWorldObject serializedObject)
  {
    SerializedObject = serializedObject;
  }

  void OnTriggerEnter(Collider c)
  {
    LevelLoader.Instance.LoadNewLevel(SerializedObject);
  }
}
