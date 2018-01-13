using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyController : MonoSingleton<PartyController> 
{
  public List<ActorLogicBase> AllPlayers = new List<ActorLogicBase>();

  public override void Initialize()
  {    
    DeliaLogic l = new DeliaLogic();
    l.InitStats("Delia", new Int2(20, 20), new Int2(5, 5), new Int2(50, 50), new Int2(30, 30), new Int2(5, 5));

    AllPlayers.Add(l);
  }

  List<ActorLogicBase> _activeParty = new List<ActorLogicBase>();
  public List<ActorLogicBase> GetActiveParty()
  {
    _activeParty.Clear();

    return _activeParty;
  }
}
