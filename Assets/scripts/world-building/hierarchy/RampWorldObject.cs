using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampWorldObject : WorldObjectBase 
{
  [HideInInspector]
  public SerializedRamp SerializedRampObject = new SerializedRamp();

  public GameObject EditorLeftWall;
  public GameObject EditorRightWall;

  public BoxCollider LeftCollider;
  public BoxCollider RightCollider;

  public void Init(SerializedRamp ramp)
  {
    SerializedRampObject = ramp;

    LeftCollider.enabled = SerializedRampObject.LeftColliderOn;
    RightCollider.enabled = SerializedRampObject.RightColliderOn;
  }

  public override void Deselect()
  {
    EditorLeftWall.SetActive(false);
    EditorRightWall.SetActive(false);
  }
}
