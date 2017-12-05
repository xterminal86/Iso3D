using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectBase : MonoBehaviour 
{
  [HideInInspector]
  public string PrefabName = string.Empty;

  public virtual void Deselect()
  {
  }
}
