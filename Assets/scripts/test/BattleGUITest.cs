using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleGUITest : MonoBehaviour 
{
  public Text InfoText;

  public List<ActorLogicBase> Players;

  void Start()
  {
    InfoText.text = "Press 'Space' to begin battle, 'Enter' to end it";

    BattleController.Instance.Initialize();
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      InfoText.text = "Battle started!";
      BattleController.Instance.BeginBattle(Players, null);
    }
    else if (Input.GetKeyDown(KeyCode.Return))
    {
      InfoText.text = "Battle ended";
      BattleController.Instance.EndBattle();
    }
  }
}
