using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActorStats", menuName = "ActorStats", order = 1)]
public class ActorStats : ScriptableObject
{
  // Each stat holds current and unmodified value

  public Int2 Strength = new Int2();
  public Int2 Defence = new Int2();
  public Int2 Speed = new Int2();

  public Int2 Hitpoints = new Int2();
  public Int2 UnityPoints = new Int2();

  public string CharName = "Unnamed Actor";

  public override string ToString()
  {
    return string.Format("[ActorStats] = Name: {5} STR: {0} DEF: {1} SPD: {2} HP: {3} UP: {4}", Strength, Defence, Speed, Hitpoints, UnityPoints, CharName);
  }
}
