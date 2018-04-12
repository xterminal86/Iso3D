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
  public Int2 Affinity = new Int2();

  [Header("Growth Rates (%)")]    
  public int StrengthGrowth = 0;
  public int DefenceGrowth = 0;
  public int SpeedGrowth = 0;
  public int AffinityGrowth = 0;

  [Header("")]    
  public int Experience = 0;
  public int Level = 1;

  public Int2 Hitpoints = new Int2();

  public string CharName = "Unnamed Actor";

  public override string ToString()
  {
    return string.Format("[ActorStats] = Name: {5} STR: {0} DEF: {1} SPD: {2} HP: {3} UP: {4}", Strength, Defence, Speed, Hitpoints, Affinity, CharName);
  }

  public void Reset()
  {
    Strength.X = Strength.Y;
    Defence.X = Defence.Y;
    Speed.X = Speed.Y;
    Affinity.X = Affinity.Y;
    Hitpoints.X = Hitpoints.Y;
    Experience = 0;
    Level = 1;
  }

  List<int> _growResults = new List<int>();
  public List<int> LevelUp()
  {
    _growResults.Clear();

    int str = RollStat(StrengthGrowth);
    int def = RollStat(DefenceGrowth);
    int spd = RollStat(SpeedGrowth);
    int afy = RollStat(AffinityGrowth);

    _growResults.Add(str);
    _growResults.Add(def);
    _growResults.Add(spd);
    _growResults.Add(afy);

    Strength.X += str;
    Defence.X += def;
    Speed.X += spd;
    Affinity.X += afy;

    Experience = 0;
    Level++;

    return _growResults;
  }

  int RollStat(int growthRate)
  {
    int increment = 0;

    int roll = UnityEngine.Random.Range(0, 101);
    if (roll <= growthRate)
    {
      increment = 1;
    }

    return increment;
  }  
}
