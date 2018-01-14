using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorLogicBase : MonoBehaviour
{  
  public ActorStats ActorStatsObject;

  protected double _battleTimer = 0.0;
  public double BattleTimer
  {
    get { return _battleTimer; }
  }

  protected double _attackTimeToReach = 0.0;
  public double AttackTimeToReach
  {
    get { return _attackTimeToReach; }
  }

  protected double _attackTimeNormalized = 0.0;
  public double AttackTimeNormalized
  {
    get { return _attackTimeNormalized; }
  }

  protected int _attackPhase = 0;
  public int AttackPhase
  {
    get { return _attackPhase; }
  }

  List<double> _attackPhasesTimes = new List<double>();
  public List<double> AttackPhasesTimes
  {
    get { return _attackPhasesTimes; }
  }

  protected bool _maxAttackPhaseReached = false;

  public bool IsInParty = false;

  public void PrepareForBattle()
  {        
    _maxAttackPhaseReached = false;
    _battleTimer = 0.0;
    _attackPhase = 0;

    _attackPhasesTimes.Clear();

    _attackTimeToReach = (double)GlobalConstants.CharacterMaxSpeed / (double)ActorStatsObject.Speed.Y;

    double firstPhaseTime = _attackTimeToReach / 3.0;

    _attackPhasesTimes.Add(firstPhaseTime);
    _attackPhasesTimes.Add(firstPhaseTime * 2.0);
    _attackPhasesTimes.Add(_attackTimeToReach);
  }

  public void StandDown()
  {    
    _maxAttackPhaseReached = false;
    _battleTimer = 0.0;
    _attackPhase = 0;
  }

  public void BattleUpdate(double dt)
  {
    if (_maxAttackPhaseReached)
    {
      return;
    }

    _battleTimer += dt;

    if (_battleTimer > _attackPhasesTimes[_attackPhase])
    {
      _attackPhase++;

      if (_attackPhase == _attackPhasesTimes.Count)
      {        
        _battleTimer = _attackTimeToReach;
        _maxAttackPhaseReached = true;
      }
    }

    _attackTimeNormalized = _battleTimer / _attackTimeToReach;
  }

  public void PrintStats()
  {
    Debug.Log(ActorStatsObject.ToString());
  }

  public virtual void UseSkill(int skillIndex)
  {
  }
}
