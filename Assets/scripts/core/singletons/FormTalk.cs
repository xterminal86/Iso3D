using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormTalk : MonoSingleton<FormTalk> 
{
  public RawImage PortraitRenderTexture;
  public Text TextComponent;

  public void TestSpeech(Camera renderCameraObject, string textToSpeak)
  {
  }
}
