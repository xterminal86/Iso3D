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
    ActorSkills[2].Available = true;
    ActorSkills[3].Available = true;
    //ActorSkills[4].Available = true;
    //ActorSkills[5].Available = true;
    ActorSkills[6].Available = true;
    //ActorSkills[7].Available = true;
    //ActorSkills[8].Available = true;

    ActorSkills[0].SkillDescription = "Light sword attack";
    ActorSkills[1].SkillDescription = "Imbue your sword with blood magic";
    ActorSkills[2].SkillDescription = "Transfer your blood to heal others";
    ActorSkills[3].SkillDescription = "Medium sword attack";
    ActorSkills[4].SkillDescription = "Summon shadow blade to strike down your enemies";
    ActorSkills[5].SkillDescription = "Attack that lowers enemy stats";
    ActorSkills[6].SkillDescription = "Strong sword attack";
    ActorSkills[7].SkillDescription = "Rest and restore your health";
    ActorSkills[8].SkillDescription = "The morning sun vanquishes the horrors of the night";
  }
}
