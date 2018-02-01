using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsTest : MonoBehaviour 
{
  public Transform ObjectToControl;

  Vector3 _originalPos = Vector3.zero;
  Vector3 _targetPos = new Vector3(5.0f, 0.0f, 5.0f);
  void Start()
  {
    _originalPos = ObjectToControl.position;
  }

  float _t = 0.0f;
  float _movingTime = 3.0f;

  bool _working = false;
  void Update()
  {
    if (Input.GetKeyUp(KeyCode.Space))
    {
      _working = true;
    }

    if (_working)
    {
      _t += Time.smoothDeltaTime;
      float dt = _t / _movingTime;
      dt = Mathf.Clamp(dt, 0.0f, 1.0f);

      Vector3 pos = Vector3.Lerp(_originalPos, _targetPos, dt);
      ObjectToControl.position = pos;

      //float d = Vector3.Distance(ObjectToControl.position, _targetPos);
      //float dt = Time.smoothDeltaTime * 2.0f;
      //ObjectToControl.position = Vector3.MoveTowards(ObjectToControl.position, _targetPos, dt);
    }
  }
}
