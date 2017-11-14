﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneLoader : MonoBehaviour 
{ 
  public LevelsList LevelToLoad;

  public bool SkipTitleScreen = false;

  void Start() 
	{ 
    DebugForm.Instance.Initialize();
    PrefabsManager.Instance.Initialize();
    CameraController.Instance.Initialize();

    LevelLoader.Instance.LoadLevel(LevelToLoad);

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

public enum LevelsList
{
  TEST = 0,
  MADE_IN_EDITOR
}
