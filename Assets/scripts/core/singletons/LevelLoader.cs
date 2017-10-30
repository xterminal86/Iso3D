using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class LevelLoader : MonoSingleton<LevelLoader> 
{
  LevelBase _levelMap;
  public LevelBase LevelMap
  {
    get { return _levelMap; }
  }

  Int3 _levelSize = new Int3(20, 10, 20);
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
        break;
    }
  }

  public void InstantiateLevel()
  {
    _levelMap.Generate();
    var hero = GameObject.Find("hero").GetComponent<HeroController3D>();
    hero.RigidbodyComponent.position = Util.MapToWorldCoordinates(new Vector3(_levelMap.PlayerPos.X, _levelMap.PlayerPos.Y, _levelMap.PlayerPos.Z));
    CameraController.Instance.SetupCamera(hero.RigidbodyComponent.position);
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
