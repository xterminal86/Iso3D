using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoSingleton<BattleController> 
{
  public List<BattleHighlightableButton> PortraitButtons = new List<BattleHighlightableButton>();

  [HideInInspector]
  public List<ActorLogicBase> EnemiesParticipating = new List<ActorLogicBase>();

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

  public void BeginBattle()
  {    
    if (_isInBattle)
    {
      return;
    }

    PrepareActors();

    _isInBattle = true;
    _isPaused = false;
  }

  public void EndBattle()
  { 
    if (!_isInBattle)
    {
      return;
    }

    UnprepareActors();

    _isInBattle = false;
    _isPaused = false;
  }

  void Update()
  {  
    if (_isPaused && Input.GetMouseButtonDown(1))
    {  
      ResumeBattle();
    }

    if (!_isPaused && _isInBattle)
    {
      UpdateActors(Time.deltaTime);
    }
  }

  void UnprepareActors()
  {
    int index = 0;
    foreach (var item in PartyController.Instance.GetActiveParty)
    {
      item.StandDown();

      foreach (var marker in PortraitButtons[index].AttackPhaseMarkers)
      {
        marker.SetActive(false);  
      }

      PortraitButtons[index].gameObject.SetActive(false);
      PortraitButtons[index].Deselect();
      index++;
    }
  }

  void PrepareActors()
  {
    int index = 0;
    foreach (var item in PartyController.Instance.GetActiveParty)
    {
      item.PrepareForBattle();
      PortraitButtons[index].Prepare(item);
      PortraitButtons[index].gameObject.SetActive(true);
      index++;
    }
  }

  void UpdateActors(double dt)
  {
    foreach (var item in PartyController.Instance.GetActiveParty)
    {
      item.BattleUpdate(dt);
    }
  }

  BattleHighlightableButton _selectedPortrait;
  public void PauseBattle(BattleHighlightableButton selectedPortrait)
  {
    Debug.Log(selectedPortrait.CurrentActor + " paused");
    _selectedPortrait = selectedPortrait;
    _isPaused = true;
  }

  public void ResumeBattle()
  {
    _selectedPortrait.Deselect();
    _isPaused = false;
  }
}
