using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializedWorldObject
{
  public SerializedVector3 WorldPosition = SerializedVector3.Zero;
  public float RotationAngle = 0.0f;
  public string PrefabName = string.Empty;
}
