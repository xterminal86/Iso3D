using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyController : MonoSingleton<PartyController> 
{
  public List<ActorLogicBase> AllPlayers = new List<ActorLogicBase>();

  Dictionary<string, ActorLogicBase> _actorLogicByName = new Dictionary<string, ActorLogicBase>();
  public override void Initialize()
  {
    foreach (var item in AllPlayers)
    {
      _actorLogicByName.Add(item.ActorStatsObject.CharName, item);
    }
  }

  List<ActorLogicBase> _activeParty = new List<ActorLogicBase>();
  public List<ActorLogicBase> GetActiveParty
  {
    get { return _activeParty; }
  }

  public void AddToParty(string characterName)
  {
    if (_activeParty.Count == 3)
    {
      Debug.LogWarning("Party is full!");
      return;
    }

    foreach (var item in _actorLogicByName)
    {
      if (item.Key == characterName)
      {
        if (item.Value.IsInParty)
        {
          Debug.LogWarning("Character " + characterName + " is already in party!");
          return;
        }

        item.Value.IsInParty = true;
        _activeParty.Add(item.Value);
        return;
      }
    }

    Debug.LogWarning("No character named " + characterName + " exists!");
  }

  public void RemoveFromParty(string characterName)
  {
    if (_activeParty.Count == 0)
    {
      Debug.LogWarning("Party is empty!");
      return;
    }

    for (int i = 0; i < _activeParty.Count; i++)
    {
      if (_activeParty[i].ActorStatsObject.CharName == characterName)
      {
        _activeParty[i].IsInParty = false;
        _activeParty.RemoveAt(i);
        return;
      }
    }
  }
}
