using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameGen : MonoBehaviour 
{
  public Text TextObject;
  public Text InfoText;

  string _vowels = "AEIOUY";
  string _consonants = "BCDFGHJKLMNPQRSTVWXZ";

  List<string> _endings = new List<string>()
  {
    "IA", "YA", "ND", "NT", "SH", "RSH", "EY", "URGH", "URG", "STAN"
  };

  void Awake()
  {
    InfoText.text = "Click \"Generate\" button to generate 10 \"names\"";
    TextObject.text = _text;
  }

  string _text = string.Empty;
  public void GenerateNames()
  {    
    _text = string.Empty;

    for (int i = 0; i < 10; i++)
    {
      string name = GenerateName();
      _text += name + '\n';
    }

    TextObject.text = _text;
  }

  string GenerateName()
  {
    string name = string.Empty;

    int maxLength = Random.Range(2, 4);

    int addEnding = Random.Range(0, 2);

    string syllable = string.Empty;
    string lastSyllable = string.Empty;

    for (int i = 0; i < maxLength; i++)
    {
      while (true)
      {        
        int mode = Random.Range(0, 2);

        syllable = GetSyllable(mode);

        if (syllable != lastSyllable)
        {
          break;
        }
      }

      name += syllable;
    }

    if (addEnding == 0)
    {
      int endingIndex = Random.Range(0, _endings.Count);
      string ending = _endings[endingIndex];

      name += ending;
    }

    return name;
  }

  string GetSyllable(int mode)
  {
    int vowelIndex = 0, consIndex = 0;
    string vowel = string.Empty;
    string cons = string.Empty;
    string syllable = string.Empty;
      
    switch (mode)
    {
      case 0:
        vowelIndex = Random.Range(0, _vowels.Length);
        consIndex = Random.Range(0, _consonants.Length);

        vowel = _vowels[vowelIndex].ToString();
        cons = _consonants[consIndex].ToString();

        syllable = (cons + vowel);
        break;

      case 1:
        consIndex = Random.Range(0, _consonants.Length);
        cons = _consonants[consIndex].ToString();

        syllable = cons;

        for (int i = 0; i < 2; i++)
        {
          vowelIndex = Random.Range(0, _vowels.Length);
          vowel = _vowels[vowelIndex].ToString();

          syllable += vowel;
        }

        break;
    }

    return syllable;
  }
}
