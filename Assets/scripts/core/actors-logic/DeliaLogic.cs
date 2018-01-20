using System.Collections;
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
    ActorSkills[1].Available = true;
    //ActorSkills[2].Available = true;
    ActorSkills[3].Available = true;
    //ActorSkills[4].Available = true;
    //ActorSkills[5].Available = true;
    ActorSkills[6].Available = true;
    //ActorSkills[7].Available = true;
    //ActorSkills[8].Available = true;

    ActorSkills[0].SkillDescription = "Light sword attack";
    ActorSkills[1].SkillDescription = "Block enemy attacks and counter them";
    ActorSkills[2].SkillDescription = "Display enemy information";
    ActorSkills[3].SkillDescription = "Medium sword attack";
    ActorSkills[4].SkillDescription = "Lower enemy defences";
    ActorSkills[5].SkillDescription = "Prevent enemy from attacking";
    ActorSkills[6].SkillDescription = "Strong sword attack";
    ActorSkills[7].SkillDescription = "Raise your defences";
    ActorSkills[8].SkillDescription = "Powerful melee attack";
  }
}
