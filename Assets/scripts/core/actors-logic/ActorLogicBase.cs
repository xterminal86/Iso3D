using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorLogicBase
{  
  public ActorStats ActorStatsObject = new ActorStats();

  protected double _battleControllerTickTimer = 0.0;
  protected double _timeToReach = 0.0;
  protected int _attackPhase = 0;
  protected bool _maxAttackPhaseReached = false;

  protected List<double> _attackPhasesTimes = new List<double>();

  public void InitStats(string charName, Int2 str, Int2 def, Int2 speed, Int2 hp, Int2 up)
  {    
    ActorStatsObject.CharName = charName;
    ActorStatsObject.StrengthStat.Set(str.X, str.Y);
    ActorStatsObject.DefenceStat.Set(def.X, def.Y);
    ActorStatsObject.SpeedStat.Set(speed.X, speed.Y);
    ActorStatsObject.Hitpoints.Set(hp.X, hp.Y);
    ActorStatsObject.UnityPoints.Set(up.X, up.Y);
  }

  public void PrepareForBattle()
  {    
    _attackPhasesTimes.Clear();

    _maxAttackPhaseReached = false;
    _battleControllerTickTimer = 0.0;
    _attackPhase = 0;

    _timeToReach = (double)GlobalConstants.CharacterMaxSpeed / (double)ActorStatsObject.SpeedStat.Y;

    double firstPhaseTime = _timeToReach / 3.0;

    _attackPhasesTimes.Add(firstPhaseTime);
    _attackPhasesTimes.Add(firstPhaseTime * 2.0);
    _attackPhasesTimes.Add(_timeToReach);
  }

  public void StandDown()
  {    
    _attackPhasesTimes.Clear();

    _maxAttackPhaseReached = false;
    _battleControllerTickTimer = 0.0;
    _attackPhase = 0;
  }

  public void BattleUpdate(double dt)
  {
    if (_maxAttackPhaseReached)
    {
      return;
    }

    _battleControllerTickTimer += dt;

    if (_battleControllerTickTimer > _attackPhasesTimes[_attackPhase])
    {
      _attackPhase++;

      if (_attackPhase == _attackPhasesTimes.Count)
      {        
        _battleControllerTickTimer = _timeToReach;
        _maxAttackPhaseReached = true;
      }
    }
  }

  public void OnCharacterSelect()
  {
    if (_attackPhase > 0 && !BattleController.Instance.IsPaused)
    {
      BattleController.Instance.PauseBattle(this);
    }
  }

  public void OnCharacterHighlight()
  {
    if (_attackPhase > 0 && !BattleController.Instance.IsPaused)
    {
    }
  }

  public virtual void UseSkill(int skillIndex)
  {
  }
}
