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

    int index = 0;
    for (int attackPhase = 0; attackPhase < 3; attackPhase++)
    {
      for (int i = 0; i < 3; i++)
      {
        ActorSkill s = new ActorSkill(skillNames[index], 0, false, attackPhase + 1);
        ActorSkills.Add(index, s);
        index++;
      }
    }

    ActorSkills[0].Available = true;
    ActorSkills[3].Available = true;
    ActorSkills[6].Available = true;
  }
}
