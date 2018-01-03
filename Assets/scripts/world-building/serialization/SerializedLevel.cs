using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializedLevel 
{
  public Int3 LevelSize = Int3.Zero;
  public List<SerializedFloor> FloorTiles = new List<SerializedFloor>();
  public List<SerializedWorldObject> Objects = new List<SerializedWorldObject>();

  public MapProperties MapPropertiesObject = new MapProperties();

  public void Init(int mapX, int mapY, int mapZ)
  {
    Clear();
    LevelSize.Set(mapX, mapY, mapZ);
  }

  void Clear()
  {
    LevelSize = Int3.Zero;
    FloorTiles.Clear();
    Objects.Clear();
  }
}

[Serializable]
public struct MapProperties
{
  public string MapName;
  public string MapAuthor;
  public string MapComments;

  public bool DirectionalLightEnabled;
};

[Serializable]
public class SerializedVector3
{
  public float X = 0.0f;
  public float Y = 0.0f;
  public float Z = 0.0f;

  public static SerializedVector3 Zero
  {
    get { return new SerializedVector3(0.0f, 0.0f, 0.0f); }
  }

  public SerializedVector3(float x, float y, float z)
  {    
    X = x;
    Y = y;
    Z = z;
  }

  public void Set(Vector3 arg)
  {
    X = arg.x;
    Y = arg.y;
    Z = arg.z;
  }

  public override string ToString()
  {
    return string.Format("[SerializedVector3: {0} {1} {2}]", X, Y, Z);
  }
}
