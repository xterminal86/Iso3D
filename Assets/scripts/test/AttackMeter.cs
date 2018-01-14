using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackMeter : MonoBehaviour 
{
  public Image AttackMeterImage;
  public Text TimerText;
  public Text SpeedText;
  public List<Image> PhaseMarkers;

  float _meterMaxWidth = 980.0f;

  public int Speed = 100;

  double _timeToReach = 0.0;
  double _meterFillFraction = 0.0;
  double _normalizedTime = 0.0f;
  double _meterPhase = 0.0;
  List<double> _phases = new List<double>();
  void Start()
  { 
    SpeedText.text = string.Format("Speed: {0}", Speed);

    _timeToReach = (double)GlobalConstants.CharacterMaxSpeed / (double)Speed;  

    _meterPhase = _timeToReach / 3.0;

    _phases.Add(_meterPhase);
    _phases.Add(_meterPhase * 2.0);
    _phases.Add(_timeToReach);

    Debug.Log("Time to reach with speed " + Speed + " = " + _timeToReach + " seconds");
  }

  double _tickTimer = 0;
  bool _isPaused = false;
  bool _isReached = false;
  Vector2 _meterSize = Vector2.one;
  int _phaseToCheck = 0;
  void Update()
  {
    if (!_isPaused && !_isReached)
    {
      _tickTimer += Time.deltaTime;
          
      if (_tickTimer > _phases[_phaseToCheck])
      {
        Debug.Log(string.Format("Phase time {0} reached", _phases[_phaseToCheck]));

        PhaseMarkers[_phaseToCheck].color = Color.yellow;

        _phaseToCheck++;

        if (_phaseToCheck == _phases.Count)
        {
          _tickTimer = _timeToReach;

          _isReached = true;
        }
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
