using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseObjectProperties : MonoBehaviour 
{
  public Text PrefabName;

  public virtual void Init(WorldObjectBase gameObject)
  {
    PrefabName.text = " " + gameObject.PrefabName;
  }
}
