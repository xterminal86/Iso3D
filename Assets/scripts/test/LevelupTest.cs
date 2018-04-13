using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelupTest : MonoBehaviour 
{
  public ActorStats ActorStatsRef;
  public TMP_Text TextComponent;
  public TMP_Text IncrementsText;
  public Button LevelUpButton;

  public AudioSource ExpGainSound;
  public AudioSource ExpGainSoundLoop;

  string _text = string.Empty;
  void Start()
  {
    UpdateText();
  }

  void UpdateText()
  {
    _text = string.Format("{0}\n\nHP:  {5}\nSTR: {1}\nDEF: {2}\nSPD: {3}\nAFY: {4}\n\n", ActorStatsRef.CharName, ActorStatsRef.Strength.X, ActorStatsRef.Defence.X, ActorStatsRef.Speed.X, ActorStatsRef.Affinity.X, ActorStatsRef.Hitpoints.X);
    _text += string.Format("EXP: {0}\nLVL: {1}", ActorStatsRef.Experience, ActorStatsRef.Level);
    TextComponent.text = _text;
  }

  bool _working = false;
  public void LevelUpHandler()
  {
    if (!_working)
    {
      StartCoroutine(GainExpRoutine());
      _working = true;
    }
  }

  IEnumerator GainExpRoutine()
  { 
    ExpGainSoundLoop.loop = true;      
    ExpGainSoundLoop.Play();

    int exp = 0;
    while (exp < 100)
    {
      exp++;
      ActorStatsRef.Experience = exp;

      UpdateText();

      yield return null;
    }

    ExpGainSound.Play();
    ExpGainSoundLoop.loop = false;      

    _working = false;

    var increments = ActorStatsRef.LevelUp();

    string incrementsText = "\n\n";
    foreach (var item in increments)
    {
      incrementsText += string.Format("+{0}\n", item);
    }

    IncrementsText.text = incrementsText;

    UpdateText();

    yield return null;
  }

  void OnDisable()
  {
    ActorStatsRef.Reset();
  }
}
