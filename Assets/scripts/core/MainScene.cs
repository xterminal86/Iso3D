using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour 
{
  void Awake()
  {
    LevelLoader.Instance.InstantiateLevel();
  }
}
