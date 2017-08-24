using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneLoader : MonoBehaviour 
{ 
  public ScenesList SceneToLoad;

  public bool SkipTitleScreen = false;

  void Start() 
	{ 
    PrefabsManager.Instance.Initialize();
    InputController.Instance.Initialize();

    LevelLoader.Instance.LoadLevel(SceneToLoad);

    if (SkipTitleScreen)
    {      
      SceneManager.LoadScene("main");
    }
    else
    {
      SceneManager.LoadScene("title");
    }
	}
}

public enum ScenesList
{
  TEST = 0
}
