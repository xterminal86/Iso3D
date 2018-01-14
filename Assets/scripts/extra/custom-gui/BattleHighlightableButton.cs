using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleHighlightableButton : HighlightableButton 
{  
  public Text HitpointsText;
  public Image AttackMeter;

  Vector2 _meterSize = Vector2.zero;

  public List<GameObject> AttackPhaseMarkers = new List<GameObject>();

  ActorLogicBase _currentActor;
  public ActorLogicBase CurrentActor
  {
    get { return _currentActor; }
  }

  public void Prepare(ActorLogicBase actorToDisplay)
  {
    _currentActor = actorToDisplay;

    HitpointsText.text = string.Format("{0}:{1}", _currentActor.ActorStatsObject.Hitpoints.X, _currentActor.ActorStatsObject.Hitpoints.Y);
  }

  public void Deselect()
  {
    Selected = false;

    // NormalSprite is drawn before other sprites (located above other in inspector), so it is always enabled.

    HighlightedSprite.SetActive(false);
    SelectedSprite.SetActive(false);
  }

  public override void OnMouseEnter(BaseEventData data)
  {     
    Highlighted = true;
  }

  public override void OnMouseExit(BaseEventData data)
  {   
    Highlighted = false;

    HighlightedSprite.SetActive(false);
  }

  public override void OnMouseDown(BaseEventData data)
  {
    if (!Enabled || !Highlighted || !BattleController.Instance.IsInBattle || BattleController.Instance.IsPaused || Selected || _currentActor.AttackPhase == 0)
    {
      return;
    }

    if (ClickSounds.Count > 0)
    {
      int index = Random.Range(0, ClickSounds.Count);
      ClickSounds[index].Play();
    }

    HighlightedSprite.SetActive(false);
    SelectedSprite.SetActive(true);

    BattleController.Instance.PauseBattle(this);
  }

  void Update()
  {
    if (!BattleController.Instance.IsInBattle || !Enabled)
    {
      return;
    }

    if (Highlighted && !Selected && !BattleController.Instance.IsPaused && _currentActor.AttackPhase > 0)
    {
      HighlightedSprite.SetActive(true);
    }

    UpdateAttackMeter();
  }

  Vector2 _meterFullSize = new Vector2(93.0f, 4.0f);
  void UpdateAttackMeter()
  {
    _meterSize.Set((float)_currentActor.AttackTimeNormalized * _meterFullSize.x, _meterFullSize.y);
    AttackMeter.rectTransform.sizeDelta = _meterSize;

    if (_currentActor.AttackPhase > 0)
    {
      AttackPhaseMarkers[_currentActor.AttackPhase - 1].SetActive(true);
    }
  }
}
