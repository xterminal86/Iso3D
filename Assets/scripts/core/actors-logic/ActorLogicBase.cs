using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorLogicBase : MonoBehaviour 
{
  public Animation PortraitAnimationComponent;

  [HideInInspector]
  public ActorStats ActorStatsObject = new ActorStats();

  public virtual void UseSkill(int skillIndex)
  {
  }
}
