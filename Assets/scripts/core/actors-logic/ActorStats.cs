using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ActorStats 
{
  // Each stat holds current and unmodified value

  public Int2 StrengthStat = new Int2();
  public Int2 DefenceStat = new Int2();
  public Int2 SpeedStat = new Int2();

  public Int2 Hitpoints = new Int2();
  public Int2 UnityPoints = new Int2();

  public string CharName = "Unnamed Actor";

  public ActorStats()
  {    
  }
}
