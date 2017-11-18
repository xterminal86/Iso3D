﻿using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorLevel : LevelBase 
{
  public EditorLevel()
  {     
  }

  SerializedLevel _loadedLevel;
  public override void LoadLevel()
  {   
    string level = "levels/level2";
    //string level = "levels/path-test";

    TextAsset ta = Resources.Load(level) as TextAsset;

    Stream stream = new MemoryStream(ta.bytes);

    var formatter = new BinaryFormatter();  
    _loadedLevel = (SerializedLevel)formatter.Deserialize(stream);  
    stream.Close();

    _mapX = _loadedLevel.LevelSize.X;
    _mapY = _loadedLevel.LevelSize.Y;
    _mapZ = _loadedLevel.LevelSize.Z;

    _level = new MapBlock[_mapX, _mapY, _mapZ];

    InitArray();

    FillMapBlocksInfo();
    ProcessFloorTransitions();

    _playerPos.Set(new Vector3(0.0f, 0.0f, 0.0f));
  }

  void FillMapBlocksInfo()
  {
    int x = 0;
    int y = 0;
    int z = 0;

    foreach (var item in _loadedLevel.FloorTiles)
    {
      x = (int)item.WorldPosition.X;
      y = (int)item.WorldPosition.Y;
      z = (int)item.WorldPosition.Z;

      _level[x, y, z].Texture1Name = item.TextureName;
      _level[x, y, z].SkipTransitionCheckHere = item.SkipTransitionCheck;
    }
  }

  public override void InstantiateLevel(Transform objectsHolder)
  {
    SerializedVector3 pos = SerializedVector3.Zero;
    GameObject go = null;

    int x = 0;
    int y = 0;
    int z = 0;
    foreach (var item in _loadedLevel.FloorTiles)
    {
      x = (int)item.WorldPosition.X;
      y = (int)item.WorldPosition.Y;
      z = (int)item.WorldPosition.Z;

      pos = item.WorldPosition;
      go = PrefabsManager.Instance.InstantiatePrefab("floor-template", _level[x, y, z].WorldCoordinates, Quaternion.identity);
      go.transform.parent = objectsHolder;
      FloorBehaviour fb = go.GetComponent<FloorBehaviour>();
      fb.Init(_level[x, y, z]);
    }

    foreach (var item in _loadedLevel.Objects)
    {
      x = (int)item.WorldPosition.X;
      y = (int)item.WorldPosition.Y;
      z = (int)item.WorldPosition.Z;

      pos = item.WorldPosition;
      go = PrefabsManager.Instance.InstantiatePrefab(item.PrefabName, _level[x, y, z].WorldCoordinates, Quaternion.Euler(0.0f, item.Angle, 0.0f));
      go.transform.parent = objectsHolder;
      WorldObjectBase wob = go.GetComponent<WorldObjectBase>();
      if (wob is WallWorldObject)
      {
        (wob as WallWorldObject).Init((item as SerializedWall).TextureName);
      }
    }
  }
}
