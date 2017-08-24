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

  public void LoadLevel(ScenesList scene)
  {
    switch (scene)
    {      
      case ScenesList.TEST:
        _levelMap = new TestLevel(_levelSize.X, _levelSize.Y, _levelSize.Z);
        break;
    }

    _levelMap.Generate();

    InputController.Instance.SetupCamera(_levelMap.PlayerPos);
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
