using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInZoneDetector : MonoBehaviour 
{
  public CallbackC MethodToCallOnEnter;
  public CallbackC MethodToCallOnExit;

  void OnTriggerEnter(Collider c)
  {
    if (MethodToCallOnEnter != null)
    {
      MethodToCallOnEnter(c);
    }
  }

  void OnTriggerExit(Collider c)
  {
    if (MethodToCallOnExit != null)
    {
      MethodToCallOnExit(c);
    }
  }
}
