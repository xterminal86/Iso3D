using System;
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

  protected string _levelName = string.Empty;
  public string LevelName
  {
    get { return _levelName; }
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

  protected float _playerRotation = 0.0f;
  public float PlayerRotation
  {
    get { return _playerRotation; }
  }

  public LevelBase()
  {    
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

  protected void InitArray()
  {
    for (int y = 0; y < _mapY; y++)
    {
      for (int x = 0; x < _mapX; x++)
      {
        for (int z = 0; z < _mapZ; z++)
        {
          _level[x, y, z] = new MapBlock();
          _level[x, y, z].ArrayCoordinates.Set(x, y, z);

          // Assume tile's world coordinates as default
          _level[x, y, z].WorldCoordinates.Set(x * GlobalConstants.ScaleFactor, y * GlobalConstants.ScaleFactor, z * GlobalConstants.ScaleFactor);
        }
      }
    }
  }

  protected void ProcessFloorTransitions()
  {
    for (int y = 0; y < _mapY; y++)
    {
      for (int x = 1; x < _mapX - 1; x++)
      {
        for (int z = 1; z < _mapZ - 1; z++)
        {          
          if (_level[x, y, z].SkipTransitionCheckHere || string.IsNullOrEmpty(_level[x, y, z].Texture1Name))
          {
            continue;
          }

          TransitionResult res = CheckNeighbours(_level[x, y, z]);
          if (res.TransitionMask == 0)
          {
            res = CheckCorners(_level[x, y, z]);
          }

          if (res.TransitionMask != 0)
          {            
            _level[x, y, z].TransitionMask = res.TransitionMask;
            _level[x, y, z].Texture2Name = res.Texture2Name;
          }

          //Debug.Log("Got " + GlobalConstants.TransitionTypeByMask[mask]);
        }
      }
    }
  }

  protected TransitionResult CheckCorners(MapBlock block)
  {
    int mask = 0;

    int x = (int)block.ArrayCoordinates.x;
    int y = (int)block.ArrayCoordinates.y;
    int z = (int)block.ArrayCoordinates.z;

    int lx = (int)block.ArrayCoordinates.x - 1;
    int hx = (int)block.ArrayCoordinates.x + 1;
    int lz = (int)block.ArrayCoordinates.z - 1;
    int hz = (int)block.ArrayCoordinates.z + 1;

    var thisFloor = block.Texture1Name;
    string texture2 = string.Empty;

    int[,] maskArray = new int[3, 3];

    // Change in z = change in y in mask array

    string nameToCheck = _level[lx, y, lz].Texture1Name;
    if (!nameToCheck.Equals(thisFloor) && !string.IsNullOrEmpty(nameToCheck))
    {
      texture2 = nameToCheck;
      maskArray[0, 0] = 1;
      _level[lx, y, lz].SkipTransitionCheckHere = true;
    }

    nameToCheck = _level[lx, y, hz].Texture1Name;

    if (!nameToCheck.Equals(thisFloor) && !string.IsNullOrEmpty(nameToCheck))
    {
      texture2 = nameToCheck;
      maskArray[0, 2] = 1;
      _level[lx, y, hz].SkipTransitionCheckHere = true;
    }

    nameToCheck = _level[hx, y, lz].Texture1Name;

    if (!nameToCheck.Equals(thisFloor) && !string.IsNullOrEmpty(nameToCheck))
    {
      texture2 = nameToCheck;
      maskArray[2, 0] = 1;
      _level[hx, y, lz].SkipTransitionCheckHere = true;
    }

    nameToCheck = _level[hx, y, hz].Texture1Name;

    if (!nameToCheck.Equals(thisFloor) && !string.IsNullOrEmpty(nameToCheck))
    {
      texture2 = nameToCheck;
      maskArray[2, 2] = 1;
      _level[hx, y, hz].SkipTransitionCheckHere = true;
    }

    string maskStr = "";

    //Debug.Log("Corners check");

    string output = string.Format("At {0}\n", block.ArrayCoordinates);
    for (int x2 = 0; x2 < 3; x2++)
    {
      for (int y2 = 0; y2 < 3; y2++)
      {
        output += maskArray[x2, y2].ToString();
        maskStr += maskArray[x2, y2].ToString();
      }

      output += "\n";
    }

    mask = int.Parse(maskStr);

    //Debug.Log(GlobalConstants.TransitionTypeByMask[mask]);

    return new TransitionResult(mask, thisFloor, texture2);
  }

  protected TransitionResult CheckNeighbours(MapBlock block)
  {
    int mask = 0;

    int x = (int)block.ArrayCoordinates.x;
    int y = (int)block.ArrayCoordinates.y;
    int z = (int)block.ArrayCoordinates.z;

    int lx = (int)block.ArrayCoordinates.x - 1;
    int hx = (int)block.ArrayCoordinates.x + 1;
    int lz = (int)block.ArrayCoordinates.z - 1;
    int hz = (int)block.ArrayCoordinates.z + 1;

    string thisFloor = block.Texture1Name;
    string texture2 = string.Empty;

    int[,] maskArray = new int[3, 3];

    // Change in z = change in y in mask array

    string nameToCheck = _level[x, y, hz].Texture1Name;

    if (!nameToCheck.Equals(thisFloor) && !string.IsNullOrEmpty(nameToCheck))
    {
      texture2 = nameToCheck;
      maskArray[1, 2] = 1;
      _level[x, y, hz].SkipTransitionCheckHere = true;
    }

    nameToCheck = _level[x, y, lz].Texture1Name;

    if (!nameToCheck.Equals(thisFloor) && !string.IsNullOrEmpty(nameToCheck))
    {
      texture2 = nameToCheck;
      maskArray[1, 0] = 1;
      _level[x, y, lz].SkipTransitionCheckHere = true;
    }

    nameToCheck = _level[hx, y, z].Texture1Name;

    if (!nameToCheck.Equals(thisFloor) && !string.IsNullOrEmpty(nameToCheck))
    {
      texture2 = nameToCheck;
      maskArray[2, 1] = 1;
      _level[hx, y, z].SkipTransitionCheckHere = true;
    }

    nameToCheck = _level[lx, y, z].Texture1Name;

    if (!nameToCheck.Equals(thisFloor) && !string.IsNullOrEmpty(nameToCheck))
    {
      texture2 = nameToCheck;
      maskArray[0, 1] = 1;
      _level[lx, y, z].SkipTransitionCheckHere = true;
    }

    string maskStr = "";

    //Debug.Log("Neighbours check");

    string output = string.Format("At {0}\n", block.ArrayCoordinates);
    for (int x2 = 0; x2 < 3; x2++)
    {
      for (int y2 = 0; y2 < 3; y2++)
      {
        output += maskArray[x2, y2].ToString();
        maskStr += maskArray[x2, y2].ToString();
      }

      output += "\n";
    }

    mask = int.Parse(maskStr);

    //Debug.Log(GlobalConstants.TransitionTypeByMask[mask]);

    return new TransitionResult(mask, thisFloor, texture2);
  }
}

public class TransitionResult
{
  public int TransitionMask = 0;
  public string Texture1Name = string.Empty;
  public string Texture2Name = string.Empty;

  public TransitionResult(int mask, string texture1, string texture2)
  {
    TransitionMask = mask;
    Texture1Name = texture1;
    Texture2Name = texture2;
  }
}

