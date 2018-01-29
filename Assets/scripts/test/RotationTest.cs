using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTest : MonoBehaviour 
{
  public Transform Object;

  Quaternion _from, _to;
  void Start()
  {
    _from = Quaternion.Euler(Object.eulerAngles.x, Object.eulerAngles.y, Object.eulerAngles.z);
    _to = Quaternion.Euler(Object.eulerAngles.x, Object.eulerAngles.y + 180.0f, Object.eulerAngles.z);
  }

  float _t = 0.0f;
  float _rotationTime = 3.0f;

  bool _working = false;
  void Update()
  {
    if (Input.GetKeyUp(KeyCode.Space))
    {
      _from = Quaternion.Euler(Object.eulerAngles.x, Object.eulerAngles.y, Object.eulerAngles.z);
      _to = Quaternion.Euler(Object.eulerAngles.x, Object.eulerAngles.y + 180.0f, Object.eulerAngles.z);

      Debug.Log(_from.eulerAngles + " " + _to.eulerAngles);

      _working = true;
    }

    if (_working)
    {
      _t += Time.smoothDeltaTime;
      float dt = _t / _rotationTime;
      dt = Mathf.Clamp(dt, 0.0f, 1.0f);

      Object.rotation = Quaternion.Lerp(_from, _to, dt);

      if ((int)dt == 1)
      {
        _t = 0.0f;
        _working = false;
      }
    }
  }

}
