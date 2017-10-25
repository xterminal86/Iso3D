using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLevel : LevelBase 
{
  public TestLevel(int x, int y, int z) : base(x, y, z)
  { 
  }

  public override void GenerateLevel()
  {    
    for (int x = 0; x < _mapX; x++)
    {
      for (int z = 0; z < _mapZ; z++)
      {
        Vector3 position = new Vector3(x, 0.0f, z);
        var go = PrefabsManager.Instance.InstantiatePrefab("floor-grass", position, Quaternion.identity);
        go.transform.position = position;
      }
    }

    Vector3 position2 = new Vector3(5.0f, 0.0f, 10.0f);
    var go2 = PrefabsManager.Instance.InstantiatePrefab("stairs", position2, Quaternion.identity);
    go2.transform.position = position2;

    position2.Set(5.0f, 0.0f, 11.0f);
    go2 = PrefabsManager.Instance.InstantiatePrefab("block-bricks", position2, Quaternion.identity);
    go2.transform.position = position2;

    position2.Set(5.0f, 1.0f, 11.0f);
    go2 = PrefabsManager.Instance.InstantiatePrefab("stairs", position2, Quaternion.identity);
    go2.transform.position = position2;

    _playerPos.Set(_mapX / 2, 0.0f, _mapZ / 2);
  }
}
