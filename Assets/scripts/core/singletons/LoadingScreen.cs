using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoSingleton<LoadingScreen> 
{  
  public RawImage Background;

  public Transform Holder;
  public Slider ProgressBar;

  Texture2D _texture;
  public override void Initialize()
  {
    _texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
  }

  public void ShowScreen()
  {    
    Holder.gameObject.SetActive(true);
  }

  public void HideScreen()
  {
    Holder.gameObject.SetActive(false);
  }

  WaitForEndOfFrame _wait = new WaitForEndOfFrame();
  public IEnumerator TakeScreenshotRoutine()
  {
    yield return _wait;

    _texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
    _texture.LoadRawTextureData(_texture.GetRawTextureData());
    _texture.Apply();

    Background.texture = _texture;
  }
}
