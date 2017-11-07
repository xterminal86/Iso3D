using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBase
{  
  protected MapBlock[,,] _level;
  public MapBlock[,,] Level
  {
    get { return _level; }
  }

  protected int _mapX, _mapY, _mapZ;

  public int MapX
  {
    get { return _mapX; }
  }

  public int MapY
  {
    get { return _mapY; }
  }

  public int MapZ
  {
    get { return _mapZ; }
  }

  /// <summary>
  /// Position of player (might be starting pos and saved for subsequent loading).
  /// Indicates array coordinates of a block in which player is located.
  /// Footstep sound is calcualted from Y - 1 coordinates (see the end of InputController::CameraMoveRoutine())
  /// </summary>   
  protected Int3 _playerPos = new Int3();
  public Int3 PlayerPos
  {
    get { return _playerPos; }
  }

  public LevelBase(int x, int y, int z)
  {
    _mapX = x;
    _mapY = y;
    _mapZ = z;

    _level = new MapBlock[x, y, z];

    InitArray();
  }

  /// <summary>
  /// Loads the level (in future made in editor).
  /// TODO: probably move loading code here later with no virtualization at all since loading will be standard.
  /// </summary>
  public virtual void LoadLevel()
  {
  }

  /// <summary>
  /// Instantiates level
  /// </summary>
  public virtual void InstantiateLevel(Transform objectsHolder)
  {
  }

  void InitArray()
  {
    for (int y = 0; y < _mapY; y++)
    {
      for (int x = 0; x < _mapX; x++)
      {
        for (int z = 0; z < _mapZ; z++)
        {
          _level[x, y, z] = new MapBlock();
          _level[x, y, z].ArrayCoordinates.Set(x, y, z);
          _level[x, y, z].WorldCoordinates.Set(x * GlobalConstants.ScaleFactor, y * GlobalConstants.ScaleFactor, z * GlobalConstants.ScaleFactor);
        }
      }
    }
  }
}
