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
        Vector3 position = Util.MapToWorldCoordinates(new Vector3(x, 0.0f, z));
        var go = PrefabsManager.Instance.InstantiatePrefab("floor-grass", position, Quaternion.identity);
        go.transform.position = position;
      }
    }

    Vector3 position2 = Util.MapToWorldCoordinates(new Vector3(5.0f, 0.0f, 10.0f));
    var go2 = PrefabsManager.Instance.InstantiatePrefab("stairs", position2, Quaternion.identity);
    go2.transform.position = position2;

    position2 = Util.MapToWorldCoordinates(new Vector3(5.0f, 0.0f, 11.0f));
    go2 = PrefabsManager.Instance.InstantiatePrefab("block-bricks", position2, Quaternion.identity);
    go2.transform.position = position2;

    position2 = Util.MapToWorldCoordinates(new Vector3(5.0f, 1.0f, 12.0f));
    go2 = PrefabsManager.Instance.InstantiatePrefab("block-bricks", position2, Quaternion.identity);
    go2.transform.position = position2;

    position2 = Util.MapToWorldCoordinates(new Vector3(5.0f, 1.0f, 11.0f));
    go2 = PrefabsManager.Instance.InstantiatePrefab("stairs", position2, Quaternion.identity);
    go2.transform.position = position2;

    position2 = Util.MapToWorldCoordinates(new Vector3(6.0f, 2.0f, 12.0f));
    go2 = PrefabsManager.Instance.InstantiatePrefab("stairs", position2, Quaternion.AngleAxis(90.0f, Vector3.up));
    go2.transform.position = position2;

    position2 = Util.MapToWorldCoordinates(new Vector3(5.0f, 2.0f, 13.0f));
    go2 = PrefabsManager.Instance.InstantiatePrefab("stairs", position2, Quaternion.identity);
    go2.transform.position = position2;

    position2 = Util.MapToWorldCoordinates(new Vector3(6.0f, 2.0f, 13.0f));
    go2 = PrefabsManager.Instance.InstantiatePrefab("block-bricks", position2, Quaternion.identity);
    go2.transform.position = position2;

    position2 = Util.MapToWorldCoordinates(new Vector3(4.0f, 1.0f, 12.0f));
    go2 = PrefabsManager.Instance.InstantiatePrefab("block-bricks", position2, Quaternion.identity);
    go2.transform.position = position2;



    _playerPos.Set(new Vector3(_mapX / 2, 0.0f, _mapZ / 2));
  }
}
