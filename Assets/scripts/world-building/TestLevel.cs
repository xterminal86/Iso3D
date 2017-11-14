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
        _level[x, 0, z].Texture1Name = "grass-green";
      }
    }

    int middle = _mapZ / 2;
    for (int x = 1; x < _mapX - 1; x++)
    {
      _level[x, 0, middle].Texture1Name = "cobblestone";
      _level[x, 0, middle].SkipTransitionCheckHere = true;
    }

    _level[10, 0, middle - 1].Texture1Name = "cobblestone";
    _level[10, 0, middle - 1].SkipTransitionCheckHere = true;

    for (int x = 2; x < 8; x++)
    {
      for (int z = 2; z < 8; z++)
      {
        _level[x, 0, z].Texture1Name = "sand";
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
}
