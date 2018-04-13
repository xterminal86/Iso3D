using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorWorldObject : WorldObjectBase 
{
  public Transform ModelCenterPos;
  public float AnimationSpeed = 1.0f;

  public Animation AnimationComponent;

  bool _isOpen = false;

  public PlayerInZoneDetector DoorOpenZone;
  public PlayerInZoneDetector DoorCloseZone;

  void Awake()
  {
    AnimationComponent["OpenInwards"].speed = AnimationSpeed;
    AnimationComponent["OpenOutwards"].speed = AnimationSpeed;

    if (DoorOpenZone != null)
    {
      DoorOpenZone.MethodToCallOnEnter += OnDoorOpen;
    }

    if (DoorCloseZone != null)
    {
      DoorCloseZone.MethodToCallOnExit += OnDoorClose;
    }
  }

  string _animationOpenName = string.Empty;
  public void OnDoorOpen(Collider c)
  {
    if (_isOpen) return;

    var hc = c.GetComponentInParent<HeroController3D>();

    //Debug.Log(RotationAngle + " Sin: " + Mathf.Sin(RotationAngle * Mathf.Deg2Rad) + " Cos: " + Mathf.Cos(RotationAngle * Mathf.Deg2Rad));

    Vector3 v1 = new Vector3(Mathf.Cos(RotationAngle * Mathf.Deg2Rad), 0.0f, -Mathf.Sin(RotationAngle * Mathf.Deg2Rad));
    Vector3 v2 = hc.transform.position - ModelCenterPos.position;
    Vector3 res = Vector3.Cross(v1, v2);

    int sign = (int)Mathf.Sign(res.y);

    if (sign == -1)
    {      
      _animationOpenName = "OpenOutwards";
    }
    else
    {
      _animationOpenName = "OpenInwards";
    }

    AnimationComponent[_animationOpenName].time = 0.0f;
    AnimationComponent[_animationOpenName].speed = AnimationSpeed;

    AnimationComponent.Play(_animationOpenName);
    _isOpen = true;
  }	

  public void OnDoorClose(Collider c)
  {
    if (!_isOpen) return;

    AnimationComponent[_animationOpenName].time = AnimationComponent[_animationOpenName].length;
    AnimationComponent[_animationOpenName].speed = -AnimationSpeed;

    AnimationComponent.Play(_animationOpenName);
    _isOpen = false;
  }

  public override void InteractHandler()
  { 
    if (_isOpen)
    {
      AnimationComponent["OpenOutwards"].time = AnimationComponent["OpenOutwards"].length;
      AnimationComponent["OpenOutwards"].speed = -AnimationSpeed;
    }
    else
    {
      AnimationComponent["OpenOutwards"].time = 0;
      AnimationComponent["OpenOutwards"].speed = AnimationSpeed;
    }

    AnimationComponent.Play("OpenOutwards");

    _isOpen = !_isOpen;
  }
}
