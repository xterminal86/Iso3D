using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RampObjectProperties : BaseObjectProperties 
{
  public Toggle LeftColliderToggle;
  public Toggle RightColliderToggle;

  RampWorldObject _ramp;
  public override void Init(WorldObjectBase gameObject)
  {
    _ramp = gameObject as RampWorldObject;

    LeftColliderToggle.isOn = _ramp.SerializedRampObject.LeftColliderOn;
    RightColliderToggle.isOn = _ramp.SerializedRampObject.RightColliderOn;

    _ramp.EditorLeftWall.SetActive(LeftColliderToggle.isOn);
    _ramp.EditorRightWall.SetActive(RightColliderToggle.isOn);
  }

  public void LeftToggleHandler()
  {
    _ramp.SerializedRampObject.LeftColliderOn = LeftColliderToggle.isOn;
    _ramp.EditorLeftWall.SetActive(LeftColliderToggle.isOn);
  }

  public void RightToggleHandler()
  {
    _ramp.SerializedRampObject.RightColliderOn = RightColliderToggle.isOn;
    _ramp.EditorRightWall.SetActive(RightColliderToggle.isOn);
  }
}
