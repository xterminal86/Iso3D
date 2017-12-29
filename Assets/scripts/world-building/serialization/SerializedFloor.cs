using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializedFloor
{
  public SerializedVector3 WorldPosition = SerializedVector3.Zero;
  public string TextureName = string.Empty;
  public bool SkipTransitionCheck = false;
  public bool AllowBlending = false;
}
