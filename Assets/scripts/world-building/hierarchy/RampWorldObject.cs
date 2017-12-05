using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampWorldObject : WorldObjectBase 
{  
  public GameObject EditorLeftWall;
  public GameObject EditorRightWall;

  public BoxCollider LeftCollider;
  public BoxCollider RightCollider;
  public BoxCollider BackCollider;

  void Awake()
  {
    SerializedObject = new SerializedRamp();
  }

  public override void Init(SerializedWorldObject serializedObject)
  {
    SerializedObject = serializedObject;

    SerializedRamp ramp = serializedObject as SerializedRamp;

    LeftCollider.enabled = ramp.LeftColliderOn;
    RightCollider.enabled = ramp.RightColliderOn;
  }

  public override void Deselect()
  {
    EditorLeftWall.SetActive(false);
    EditorRightWall.SetActive(false);
  }
}
