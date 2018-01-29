using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsTest : MonoBehaviour 
{
  public Transform ObjectToControl;

  bool _working = false;
  void Update()
  {
    if (Input.GetKeyUp(KeyCode.Space))
    {
      _working = true;
    }

    if (_working)
    {
      ObjectToControl.position = Vector3.MoveTowards(ObjectToControl.position, new Vector3(5.0f, 0.0f, 5.0f), Time.smoothDeltaTime * 2.0f);
    }
  }
}
