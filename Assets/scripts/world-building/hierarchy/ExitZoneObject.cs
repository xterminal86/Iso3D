using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitZoneObject : WorldObjectBase 
{
  [HideInInspector]
  public string LevelNameToLoad = string.Empty;

  [HideInInspector]
  public Int3 ArrivalMapPosition = Int3.Zero;

  [HideInInspector]
  public float ArrivalCharacterOrientation = 0.0f; 
}
