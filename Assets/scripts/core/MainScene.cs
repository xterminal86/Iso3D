using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour 
{
  public Transform ObjectsHolder;

  void Awake()
  {
    LevelLoader.Instance.InstantiateLevel(ObjectsHolder);
  }
}
