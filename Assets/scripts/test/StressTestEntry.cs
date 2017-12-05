using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StressTestEntry : MonoSingleton<StressTestEntry> 
{
	void Start () 
  {
    DebugForm.Instance.Initialize();
    PrefabsManager.Instance.Initialize();
    CameraController.Instance.Initialize();
    LoadingScreen.Instance.Initialize();   

    LevelLoader.Instance.Initialize();

    SceneManager.sceneLoaded += LevelLoader.Instance.SceneLoadedHandler;

    StartCoroutine(SwitchLevelsRoutine());
	}
	
  IEnumerator SwitchLevelsRoutine()
  {
    int maxSwitches = 1000;

    int count = 0;

    SerializedExitZone zone1 = new SerializedExitZone();
    SerializedExitZone zone2 = new SerializedExitZone();

    zone1.LevelNameToLoad = "level";
    zone1.ArrivalMapPosition = new Int3(0, 0, 0);
    zone1.ArrivalCharacterAngle = 0.0f;

    zone2.LevelNameToLoad = "level2";
    zone2.ArrivalMapPosition = new Int3(0, 0, 0);
    zone2.ArrivalCharacterAngle = 0.0f;

    float timer = 0.0f;

    bool switch_ = false;

    SerializedExitZone zone;

    while (count < maxSwitches)
    {
      timer += Time.smoothDeltaTime;

      if (timer > 1.0f)
      {
        zone = !switch_ ? zone1 : zone2;

        Debug.Log("Loading scene " + zone.LevelNameToLoad);

        LevelLoader.Instance.LoadNewLevel(zone);
        timer = 0.0f;

        switch_ = !switch_;
      }

      yield return null;
    }

    Debug.Log("Max number of switches reached");

    yield return null;
  }
}
