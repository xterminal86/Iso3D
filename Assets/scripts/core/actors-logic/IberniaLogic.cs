using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IberniaLogic : ActorLogicBase 
{
  public override void Init()
  {
    string[] skillNames = new string[9] 
    { 
      "1. Sword", "2. BloodSword", "3. Transfusion", 
      "4. SwordHit", "5. DarkSword", "6. CurseStrike", 
      "7. SwordStrike", "8. Regenerate", "9. RadiantDawn" 
    };

    for (int i = 0; i < 9; i++)
    {
      ActorSkill s = new ActorSkill(skillNames[i], 0, false);
      ActorSkills.Add(i, s);
    }
  }
}
