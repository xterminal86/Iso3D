using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormTalk : MonoSingleton<FormTalk> 
{
  public RawImage PortraitRenderTexture;
  public Text TextComponent;

  public GameObject PortraitForm;
  public GameObject TextArea;

  HeroController3D _heroController;
  public void TestSpeech(HeroController3D heroController, string textToSpeak)
  {
    _heroController = heroController;
    heroController.CharPortraitCamera.gameObject.SetActive(true);

    TextComponent.text = "";

    StartCoroutine(PrintTextRoutine(textToSpeak));
  }

  IEnumerator PrintTextRoutine(string textToPrint)
  {
    PortraitForm.SetActive(true);
    TextArea.SetActive(true);

    for (int i = 0; i < textToPrint.Length; i++)
    {
      TextComponent.text += textToPrint[i];

      yield return null;
    }

    yield return new WaitForSeconds(1.0f);

    PortraitForm.SetActive(false);
    TextArea.SetActive(false);

    _heroController.CharPortraitCamera.gameObject.SetActive(false);

    yield return null;
  }
}
