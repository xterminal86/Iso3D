using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackMeter : MonoBehaviour 
{
  public Image AttackMeterImage;

  float _meterMaxWidth = 980.0f;

  public int Speed = 100;

  float _speedMeter = 0.0f;
  float _speedToReach = 0.0f;
  float _meterFillFraction = 0.0f;
  void Start()
  {
    // Assuming Speed = 100 -> 5 seconds
    _meterFillFraction = (float)Speed / 100.0f;
    _speedToReach = 5.0f / _meterFillFraction;

    InvokeRepeating("FillMeter", 0.0f, _meterFillFraction); 
  }

  void FillMeter()
  {
    if (_speedMeter >= _speedToReach)
    {
      Debug.Log("reached");
      return;
    }

    _speedMeter += _meterFillFraction;

    Debug.Log(_speedMeter);
  }

  bool _isPaused = false;
  void Update()
  {
    if (!_isPaused && Input.GetMouseButtonDown(0))
    {
      Debug.Log("paused");
      _isPaused = true;
      CancelInvoke();
    }
    else if (_isPaused && Input.GetMouseButtonDown(1))      
    {
      _isPaused = false;
      InvokeRepeating("FillMeter", 0.0f, _meterFillFraction); 
    }
  }
}
