﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackMeter : MonoBehaviour 
{
  public Image AttackMeterImage;
  public Text TimerText;
  public Text SpeedText;

  float _meterMaxWidth = 980.0f;

  public int Speed = 100;

  double _timeToReach = 0.0;
  double _meterFillFraction = 0.0;
  double _normalizedTime = 0.0f;
  void Start()
  { 
    SpeedText.text = string.Format("Speed: {0}", Speed);

    _timeToReach = GlobalConstants.InGameTick / ((Speed * GlobalConstants.InGameTick) / (double)GlobalConstants.CharacterMaxSpeed);  

    Debug.Log("Time to reach with speed " + Speed + " = " + _timeToReach + " seconds");
  }

  void TimerEvent()
  {
    Debug.Log("reached");
  }

  double _tickTimer = 0;
  double _deltaTimer = 0.0;
  bool _isPaused = false;
  bool _isReached = false;
  Vector2 _meterSize = Vector2.one;
  void Update()
  {
    if (!_isPaused && !_isReached)
    {
      _deltaTimer += Time.deltaTime;

      if (_deltaTimer > GlobalConstants.InGameTick)
      {
        _tickTimer += GlobalConstants.InGameTick;
        _deltaTimer = 0.0;
      }

      if (_tickTimer > _timeToReach)
      {
        _tickTimer = _timeToReach;

        _isReached = true;
        TimerEvent();
      }

      _normalizedTime = _tickTimer / _timeToReach;
      _meterFillFraction = _normalizedTime * _meterMaxWidth;

      _meterSize.Set((float)_meterFillFraction, 80.0f);
      AttackMeterImage.rectTransform.sizeDelta = _meterSize;
    }
        
    if (!_isPaused && Input.GetMouseButtonDown(0))
    {
      _isPaused = true;
    }
    else if (_isPaused && Input.GetMouseButtonDown(1))      
    {
      _isPaused = false;
    }

    TimerText.text = string.Format("{0:F3}", _tickTimer);
  }
}