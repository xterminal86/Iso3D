using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorLevel : LevelBase 
{  
  public EditorLevel(string levelName, Int3 pos, float angle)
  {     
    _levelName = "levels/" + levelName;
    _playerPos.Set(new Vector3(pos.X, pos.Y, pos.Z));
    _playerRotation = angle;
  }

  public override void LoadLevel()
  { 
    TextAsset ta = Resources.Load(_levelName) as TextAsset;

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
      _level[x, y, z].AllowBlending = item.AllowBlending;
    }
  }

  public override void InstantiateLevel(Transform objectsHolder)
  {
    SerializedVector3 pos = SerializedVector3.Zero;
    GameObject go = null;

    int x = 0;
    int y = 0;
    int z = 0;

    GameObject floorCombinedMeshHolder = new GameObject("FloorMesh");
    GameObject staticObjectsCombinedMeshHolder = new GameObject("StaticObjects");

    foreach (var item in _loadedLevel.FloorTiles)
    {
      x = (int)item.WorldPosition.X;
      y = (int)item.WorldPosition.Y;
      z = (int)item.WorldPosition.Z;

      pos = item.WorldPosition;

      go = PrefabsManager.Instance.InstantiatePrefab(GlobalConstants.FloorTemplatePrefabName, _level[x, y, z].WorldCoordinates, Quaternion.identity);
      if (go != null)
      {
        //go.transform.parent = objectsHolder;
        go.transform.parent = floorCombinedMeshHolder.transform;

        FloorBehaviour fb = go.GetComponent<FloorBehaviour>();
        fb.Init(_level[x, y, z]);
      }
    }

    foreach (var item in _loadedLevel.Objects)
    {
      x = (int)item.WorldPosition.X;
      y = (int)item.WorldPosition.Y;
      z = (int)item.WorldPosition.Z;

      pos = item.WorldPosition;

      // World objects are have their own world position, which is serialized in the editor.

      _level[x, y, z].WorldCoordinates.Set(pos.X * GlobalConstants.ScaleFactor, pos.Y * GlobalConstants.ScaleFactor, pos.Z * GlobalConstants.ScaleFactor);

      go = PrefabsManager.Instance.InstantiatePrefab(item.PrefabName, _level[x, y, z].WorldCoordinates, Quaternion.Euler(0.0f, item.RotationAngle, 0.0f));
      if (go != null)
      {
        //go.transform.parent = objectsHolder;
        go.transform.parent = staticObjectsCombinedMeshHolder.transform;

        WorldObjectBase wob = go.GetComponent<WorldObjectBase>();
        wob.RotationAngle = item.RotationAngle;

        wob.Init(item);
        wob.PostProcess();
      }
    }

    floorCombinedMeshHolder.CombineMeshes();

    // FIXME: produces error
    //staticObjectsCombinedMeshHolder.CombineMeshes();
  }
}
