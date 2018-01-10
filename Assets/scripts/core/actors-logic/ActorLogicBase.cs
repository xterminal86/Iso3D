using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActorLogicBase : MonoBehaviour 
{
  public Animation PortraitAnimationComponent;
  public Image AttackMeterImage;
  public List<GameObject> AttackPhaseMarkers;
  public HighlightableControl PortraitButton;

  [HideInInspector]
  public ActorStats ActorStatsObject = new ActorStats();

  protected double _battleControllerTickTimer = 0.0;
  protected double _timeToReach = 0.0;
  protected int _attackPhase = 0;
  protected bool _maxAttackPhaseReached = false;

  protected List<double> _attackPhasesTimes = new List<double>();

  public void PrepareForBattle()
  {
    _attackPhasesTimes.Clear();

    _maxAttackPhaseReached = false;
    _battleControllerTickTimer = 0.0;
    _attackPhase = 0;

    _timeToReach = GlobalConstants.InGameTick / ((ActorStatsObject.SpeedStat.Y * GlobalConstants.InGameTick) / (double)GlobalConstants.CharacterMaxSpeed);

    double firstPhaseTime = _timeToReach / 3.0;

    _attackPhasesTimes.Add(firstPhaseTime);
    _attackPhasesTimes.Add(firstPhaseTime * 2.0);
    _attackPhasesTimes.Add(_timeToReach);
  }

  public void StandDown()
  {
    foreach (var marker in AttackPhaseMarkers)
    {
      marker.SetActive(false);
    }

    _attackPhasesTimes.Clear();

    _maxAttackPhaseReached = false;
    _battleControllerTickTimer = 0.0;
    _attackPhase = 0;

    _meterSize.Set(0.0f, _meterDefaultHeight);
    AttackMeterImage.rectTransform.sizeDelta = _meterSize;
  }

  const float _meterMaxWidth = 93.0f;
  const float _meterDefaultHeight = 4.0f;

  Vector2 _meterSize = Vector2.zero;

  public void BattleUpdate(double dt)
  {
    if (_maxAttackPhaseReached)
    {
      return;
    }

    _battleControllerTickTimer += dt;

    if (_battleControllerTickTimer > _attackPhasesTimes[_attackPhase])
    {
      AttackPhaseMarkers[_attackPhase].SetActive(true);

      _attackPhase++;

      if (_attackPhase == _attackPhasesTimes.Count)
      {        
        _battleControllerTickTimer = _timeToReach;
        _maxAttackPhaseReached = true;
      }
    }

    double _normalizedTime = _battleControllerTickTimer / _timeToReach;
    double _meterFillFraction = _normalizedTime * _meterMaxWidth;

    _meterSize.Set((float)_meterFillFraction, _meterDefaultHeight);
    AttackMeterImage.rectTransform.sizeDelta = _meterSize;
  }

  public void OnCharacterSelect()
  {
    Debug.Log(_attackPhase);

    if (_attackPhase > 0)
    {
      BattleController.Instance.PauseBattle(this);
    }
  }

  public virtual void UseSkill(int skillIndex)
  {
  }
}
