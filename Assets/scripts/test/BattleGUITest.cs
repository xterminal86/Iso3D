using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleGUITest : MonoBehaviour 
{
  public Text InfoText;

  void Start()
  {
    InfoText.text = "Press 'Space' to begin battle, 'Enter' to end it";

    LevelLoader.Instance.Initialize();
    CameraController.Instance.Initialize();
    PartyController.Instance.Initialize();

    PartyController.Instance.AddToParty("Delia");
    //PartyController.Instance.AddToParty("Ibernia");
    //PartyController.Instance.AddToParty("Shijima");

    BattleController.Instance.Initialize();
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      InfoText.text = "Battle started!";
      BattleController.Instance.BeginBattle();
    }
    else if (Input.GetKeyDown(KeyCode.Return))
    {
      InfoText.text = "Battle ended";
      BattleController.Instance.EndBattle();
    }
  }
}
