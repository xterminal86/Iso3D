using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShijimaLogic : ActorLogicBase 
{
  public override void Init()
  {
    string[] skillNames = new string[9] 
    { 
      "1. Slash", "2. ArcSlash", "3. Cleanse", 
      "4. DoubleSlash", "5. Flicker", "6. Prayer", 
      "7. TripleSlash", "8. Protect", "9. Thunderstorm" 
    };

    for (int i = 0; i < 9; i++)
    {
      ActorSkill s = new ActorSkill(skillNames[i], 0, false);
      ActorSkills.Add(i, s);
    }
  }
}
