using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharData 
{
  // Each stat holds current and maximum value

  public Int2 StrengthStat = new Int2();
  public Int2 DefenceStat = new Int2();
  public Int2 SpeedStat = new Int2();
  public Int2 SenseStat = new Int2();

  public Int2 Hitpoints = new Int2();

  public string CharName = "Unnamed Actor";

  public CharData()
  {    
  }

  public CharData(string name, int str, int def, int spd, int sen, int hp)
  {
    CharName = name;

    StrengthStat.Set(str, str);
    DefenceStat.Set(def, def);
    SpeedStat.Set(spd, spd);
    SenseStat.Set(sen, sen);
    Hitpoints.Set(hp, hp);
  }
}
