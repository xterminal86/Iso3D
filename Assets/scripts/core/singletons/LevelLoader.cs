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
        SerializedExitZone zone = new SerializedExitZone();

        zone.LevelNameToLoad = "level";
        zone.ArrivalMapPosition.Set(0, 0, 0);
        zone.ArrivalCharacterAngle = 0.0f;

        _levelMap = new EditorLevel(zone.LevelNameToLoad, zone.ArrivalMapPosition, zone.ArrivalCharacterAngle);
        _levelMap.LoadLevel();
        break;
    }
  }

  public void SceneLoadedHandler(Scene s, LoadSceneMode mode)
  {
    Debug.Log("Level " + _levelMap.LevelName + " loaded");

    SceneManager.sceneLoaded -= LevelLoader.Instance.SceneLoadedHandler;

    InstantiateLevel();
  }

  public void InstantiateLevel()
  {
    GameObject holder = new GameObject("MapHolder");
    _levelMap.InstantiateLevel(holder.transform);
    var hero = GameObject.Find("hero").GetComponentInChildren<HeroController3D>();
    hero.InitPlayerPosition(_levelMap.PlayerPos, _levelMap.PlayerRotation);
    CameraController.Instance.SetupCamera(hero.RigidbodyComponent.position);
  }

  bool _isNewLevelBeingLoaded = false;
  public bool IsNewLevelBeingLoaded
  {
    get { return _isNewLevelBeingLoaded; }
  }

  public void LoadNewLevel(SerializedExitZone exitZone)
  {
    if (!_isNewLevelBeingLoaded)
    {
      _isNewLevelBeingLoaded = true;
      StartCoroutine(LoadLevelRoutine(exitZone));
    }
  }

  IEnumerator LoadLevelRoutine(SerializedExitZone exitZone)
  { 
    // Without this game crashes after several zone transitions
    System.GC.Collect();

    LoadingScreen.Instance.ProgressBar.value = 0.0f;

    yield return LoadingScreen.Instance.TakeScreenshotRoutine();

    LoadingScreen.Instance.ShowScreen();

    yield return new WaitForSeconds(0.25f);

    _levelMap = new EditorLevel(exitZone.LevelNameToLoad, exitZone.ArrivalMapPosition, exitZone.ArrivalCharacterAngle);
    _levelMap.LoadLevel();

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

    _isNewLevelBeingLoaded = false;

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
