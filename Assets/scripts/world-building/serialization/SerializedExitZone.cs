using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializedExitZone : SerializedWorldObject 
{
  public string LevelNameToLoad = string.Empty;
  public Int3 ArrivalMapPosition = Int3.Zero;
  public float ArrivalCharacterAngle = 0.0f;

  public override string ToString()
  {
    return string.Format("[SerializedExitZone] => {0} {1} {2}", LevelNameToLoad, ArrivalMapPosition, ArrivalCharacterAngle);
  }
}
