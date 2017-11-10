using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLevel : LevelBase 
{
  public TestLevel(int x, int y, int z) : base(x, y, z)
  { 
  }

  public override void LoadLevel()
  {
    for (int x = 0; x < _mapX; x++)
    {
      for (int z = 0; z < _mapZ; z++)
      {
        _level[x, 0, z].Texture1 = GlobalConstants.TEXTURE_TYPE.GRASS;
      }
    }

    //_level[2, 0, 5].Texture1 = GlobalConstants.TEXTURE_TYPE.COBBLESTONE;

    int middle = _mapZ / 2;
    for (int x = 1; x < _mapX - 1; x++)
    {
      _level[x, 0, middle].Texture1 = GlobalConstants.TEXTURE_TYPE.COBBLESTONE;
      _level[x, 0, middle].SkipTransitionCheckHere = true;
    }

    _level[10, 0, middle - 1].Texture1 = GlobalConstants.TEXTURE_TYPE.COBBLESTONE;
    _level[10, 0, middle - 1].SkipTransitionCheckHere = true;

    for (int x = 2; x < 8; x++)
    {
      for (int z = 2; z < 8; z++)
      {
        _level[x, 0, z].Texture1 = GlobalConstants.TEXTURE_TYPE.SAND;
        _level[x, 0, z].SkipTransitionCheckHere = true;
      }
    }

    ProcessFloorTransitions();

    _playerPos.Set(new Vector3(_mapX / 2, 0.0f, _mapZ / 2));
  }

  public override void InstantiateLevel(Transform objectsHolder)
  {
    for (int y = 0; y < _mapY; y++)
    {
      for (int x = 0; x < _mapX; x++)
      {
        for (int z = 0; z < _mapZ; z++)
        {   
          var go = PrefabsManager.Instance.InstantiatePrefab("floor-template", _level[x, y, z].WorldCoordinates, Quaternion.identity);
          go.transform.parent = objectsHolder;
          FloorBehaviour fb = go.GetComponent<FloorBehaviour>();
          fb.Init(_level[x, y, z]);
        }
      }
    }
  }

  void ProcessFloorTransitions()
  {
    for (int y = 0; y < _mapY; y++)
    {
      for (int x = 1; x < _mapX - 1; x++)
      {
        for (int z = 1; z < _mapZ - 1; z++)
        {          
          if (_level[x, y, z].SkipTransitionCheckHere)
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
            _level[x, y, z].Texture2 = res.Texture2;
          }

          //Debug.Log("Got " + GlobalConstants.TransitionTypeByMask[mask]);
        }
      }
    }
  }

  TransitionResult CheckCorners(MapBlock block)
  {
    int mask = 0;

    int x = (int)block.ArrayCoordinates.x;
    int y = (int)block.ArrayCoordinates.y;
    int z = (int)block.ArrayCoordinates.z;

    int lx = (int)block.ArrayCoordinates.x - 1;
    int hx = (int)block.ArrayCoordinates.x + 1;
    int lz = (int)block.ArrayCoordinates.z - 1;
    int hz = (int)block.ArrayCoordinates.z + 1;

    var thisFloor = block.Texture1;
    GlobalConstants.TEXTURE_TYPE texture2 = GlobalConstants.TEXTURE_TYPE.NONE;

    int[,] maskArray = new int[3, 3];

    // Change in z = change in y in mask array

    if (_level[lx, y, lz].Texture1 != thisFloor)
    {
      texture2 = _level[lx, y, lz].Texture1;
      maskArray[0, 0] = 1;
      //_level[lx, y, lz].SkipTransitionCheckHere = true;
    }

    if (_level[lx, y, hz].Texture1 != thisFloor)
    {
      texture2 = _level[lx, y, hz].Texture1;
      maskArray[0, 2] = 1;
      //_level[lx, y, hz].SkipTransitionCheckHere = true;
    }

    if (_level[hx, y, lz].Texture1 != thisFloor)
    {
      texture2 = _level[hx, y, lz].Texture1;
      maskArray[2, 0] = 1;
      //_level[hx, y, lz].SkipTransitionCheckHere = true;
    }

    if (_level[hx, y, hz].Texture1 != thisFloor)
    {
      texture2 = _level[hx, y, hz].Texture1;
      maskArray[2, 2] = 1;
      //_level[hx, y, hz].SkipTransitionCheckHere = true;
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

  TransitionResult CheckNeighbours(MapBlock block)
  {
    int mask = 0;

    int x = (int)block.ArrayCoordinates.x;
    int y = (int)block.ArrayCoordinates.y;
    int z = (int)block.ArrayCoordinates.z;

    int lx = (int)block.ArrayCoordinates.x - 1;
    int hx = (int)block.ArrayCoordinates.x + 1;
    int lz = (int)block.ArrayCoordinates.z - 1;
    int hz = (int)block.ArrayCoordinates.z + 1;

    GlobalConstants.TEXTURE_TYPE thisFloor = block.Texture1;
    GlobalConstants.TEXTURE_TYPE texture2 = GlobalConstants.TEXTURE_TYPE.NONE;

    int[,] maskArray = new int[3, 3];

    // Change in z = change in y in mask array

    if (_level[x, y, hz].Texture1 != thisFloor)
    {
      texture2 = _level[x, y, hz].Texture1;
      maskArray[1, 2] = 1;
      //_level[x, y, hz].SkipTransitionCheckHere = true;
    }

    if (_level[x, y, lz].Texture1 != thisFloor)
    {
      texture2 = _level[x, y, lz].Texture1;
      maskArray[1, 0] = 1;
      //_level[x, y, lz].SkipTransitionCheckHere = true;
    }

    if (_level[hx, y, z].Texture1 != thisFloor)
    {
      texture2 = _level[hx, y, z].Texture1;
      maskArray[2, 1] = 1;
      //_level[hx, y, z].SkipTransitionCheckHere = true;
    }

    if (_level[lx, y, z].Texture1 != thisFloor)
    {
      texture2 = _level[lx, y, z].Texture1;
      maskArray[0, 1] = 1;
      //_level[lx, y, z].SkipTransitionCheckHere = true;
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
  public GlobalConstants.TEXTURE_TYPE Texture1 = GlobalConstants.TEXTURE_TYPE.NONE;
  public GlobalConstants.TEXTURE_TYPE Texture2 = GlobalConstants.TEXTURE_TYPE.NONE;

  public TransitionResult(int mask, GlobalConstants.TEXTURE_TYPE texture1, GlobalConstants.TEXTURE_TYPE texture2)
  {
    TransitionMask = mask;
    Texture1 = texture1;
    Texture2 = texture2;
  }
}
