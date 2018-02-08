using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour 
{
  public PlayerInZoneDetector PlayerDetector;

  void Awake()
  {
    PlayerDetector.MethodToCallOnEnter += PlayerDetected;
    PlayerDetector.MethodToCallOnExit += PlayerExited;
  }

  void PlayerDetected(Collider c)
  {
    Debug.Log("detected " + c);
  }

  void PlayerExited(Collider c)
  {
    Debug.Log("exited " + c);
  }
}
