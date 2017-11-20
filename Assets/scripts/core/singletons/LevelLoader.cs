using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoSingleton<LevelLoader> 
{
  LevelBase _levelMap;
  public LevelBase LevelMap
  {
    get { return _levelMap; }
  }

  Int3 _levelSize = new Int3(20, 1, 20);
  public Int3 LevelSize
  {
    get { return _levelSize; }
  }

  public void LoadLevel(LevelsList scene)
  {
    switch (scene)
    {      
      case LevelsList.TEST:
        _levelMap = new TestLevel(_levelSize.X, _levelSize.Y, _levelSize.Z);
        _levelMap.LoadLevel();
        break;

      case LevelsList.MADE_IN_EDITOR:
        _levelMap = new EditorLevel();
        _levelMap.LoadLevel();
        break;
    }
  }

  public void SceneLoadedHandler(Scene s, LoadSceneMode mode)
  {
    InstantiateLevel();
  }

  public void InstantiateLevel()
  {
    GameObject holder = new GameObject("MapHolder");
    _levelMap.InstantiateLevel(holder.transform);
    var hero = GameObject.Find("hero").GetComponent<HeroController3D>();
    hero.SetPlayerPosition(_levelMap.PlayerPos);
    CameraController.Instance.SetupCamera(hero.RigidbodyComponent.position);
  }

  bool _loading = false;
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.R) && !_loading)
    {
      _loading = true;
      StartCoroutine(LoadLevelRoutine());
    }
  }

  IEnumerator LoadLevelRoutine()
  { 
    yield return LoadingScreen.Instance.TakeScreenshotRoutine();

    LoadingScreen.Instance.ShowScreen();

    //yield return new WaitForSeconds(1);

    LoadLevel(LevelsList.TEST);
    var res = SceneManager.LoadSceneAsync("main");

    res.allowSceneActivation = false;

    while (!res.isDone)
    {
      float percentage = Mathf.Clamp01(res.progress / 0.9f);
      LoadingScreen.Instance.ProgressBar.value = percentage;

      if (percentage >= 1.0f)
      {
        res.allowSceneActivation = true;
      }

      yield return null;
    }

    LoadingScreen.Instance.HideScreen();
    InstantiateLevel();

    _loading = false;

    yield return null;
  }

  /// <summary>
  /// Gets the block by array coordinates.
  /// </summary>
  /// <returns>The block by coordinates or null if coordinates are out of bounds</returns>
  public MapBlock GetBlockByCoordinates(Int3 coords)
  {
    MapBlock block = null;

    if (coords.X >= 0 && coords.X < _levelSize.X
     && coords.Y >= 0 && coords.Y < _levelSize.Y
     && coords.Z >= 0 && coords.Z < _levelSize.Z)
    {
      block = _levelMap.Level[coords.X, coords.Y, coords.Z];
    }

    return block;
  }
}
