﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliaLogic : ActorLogicBase 
{  
  public override void Init()
  {
    string[] skillNames = new string[9] 
    { 
      "1. Sword", "2. Riposte", "3. Observe", 
      "4. SwordHit", "5. Shatter", "6. Tackle", 
      "7. SwordStrike", "8. BattleCry", "9. Assault" 
    };

    for (int i = 0; i < 9; i++)
    {
      ActorSkill s = new ActorSkill(skillNames[i], 0, false);
      ActorSkills.Add(i, s);
    }
  }
}
