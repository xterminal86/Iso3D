using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoSingleton<BattleController> 
{
  [HideInInspector]
  public List<ActorLogicBase> PlayersParticipating = new List<ActorLogicBase>();

  [HideInInspector]
  public List<ActorLogicBase> EnemiesParticipating = new List<ActorLogicBase>();

  double _deltaTimer = 0.0;

  bool _isPaused = false;
  public bool IsPaused
  {
    get { return _isPaused; }
  }

  bool _isInBattle = false;
  public bool IsInBattle
  {
    get { return _isInBattle; }
  }

  public void BeginBattle(List<ActorLogicBase> players, List<ActorLogicBase> enemies)
  {    
    if (_isInBattle)
    {
      return;
    }

    PlayersParticipating = players;
    EnemiesParticipating = enemies;

    _deltaTimer = 0.0;

    PrepareActors();

    _isInBattle = true;
    _isPaused = false;
  }

  public void EndBattle()
  {    
    _isInBattle = false;

    UnprepareActors();
  }

  void Update()
  {  
    if (_isPaused && Input.GetMouseButtonDown(1))
    {  
      ResumeBattle();
    }

    if (!_isPaused && _isInBattle)
    {
      _deltaTimer += Time.deltaTime;

      UpdateActors(Time.deltaTime);
    }
  }

  void UnprepareActors()
  {
    foreach (var actor in PlayersParticipating)
    {
      actor.StandDown();
    }
  }

  void PrepareActors()
  {
    foreach (var actor in PlayersParticipating)
    {
      actor.PrepareForBattle();
    }
  }

  void UpdateActors(double dt)
  {
    foreach (var actor in PlayersParticipating)
    {
      actor.BattleUpdate(dt);
    }
  }

  ActorLogicBase _selectedActor;
  public void PauseBattle(ActorLogicBase causer)
  {
    _selectedActor = causer;
    _isPaused = true;
  }

  public void ResumeBattle()
  {
    _selectedActor.PortraitButton.ControlGroupRef.ResetControls();
    _isPaused = false;
  }
}
