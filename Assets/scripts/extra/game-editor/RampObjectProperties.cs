using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RampObjectProperties : BaseObjectProperties 
{
  public Toggle LeftColliderToggle;
  public Toggle RightColliderToggle;

  RampWorldObject _rampBehaviour;
  SerializedRamp _rampSerialized;
  public override void Init(WorldObjectBase gameObject)
  {
    _rampBehaviour = gameObject as RampWorldObject;
    _rampSerialized = _rampBehaviour.SerializedObject as SerializedRamp;

    LeftColliderToggle.isOn = _rampSerialized.LeftColliderOn;
    RightColliderToggle.isOn = _rampSerialized.RightColliderOn;

    _rampBehaviour.EditorLeftWall.SetActive(LeftColliderToggle.isOn);
    _rampBehaviour.EditorRightWall.SetActive(RightColliderToggle.isOn);
  }

  public void LeftToggleHandler()
  {
    _rampSerialized.LeftColliderOn = LeftColliderToggle.isOn;
    _rampBehaviour.EditorLeftWall.SetActive(LeftColliderToggle.isOn);
  }

  public void RightToggleHandler()
  {
    _rampSerialized.RightColliderOn = RightColliderToggle.isOn;
    _rampBehaviour.EditorRightWall.SetActive(RightColliderToggle.isOn);
  }
}
