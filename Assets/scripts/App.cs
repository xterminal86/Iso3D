using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class App : MonoBehaviour 
{
  public Transform ObjectsHolder;

  public GameObject Tile;
  public GameObject ObjectColumn;
  public GameObject ObjectCube;

  public Actor ActorObject;

  int[,] _map;

  void Awake()
  {
    _map = new int[GlobalConstants.MapWidth, GlobalConstants.MapHeight];

    CreateGrid();
    PlaceObjects();
    PlaceActor();
  }

  void CreateGrid()
  {
    IsoObjectMapInfo info = new IsoObjectMapInfo(0, 0);

    for (int x = 0; x < GlobalConstants.MapWidth; x++)
    {
      for (int y = 0; y < GlobalConstants.MapHeight; y++)
      {
        info.Init(x, y);

        GameObject go = (GameObject)Instantiate(Tile, info.WorldPosition, Quaternion.identity);
        go.transform.localPosition = info.WorldPosition;
        go.transform.SetParent(ObjectsHolder);
        go.GetComponent<SpriteRenderer>().sortingOrder = -1;
      }
    }
  }

  void PlaceObjects()
  {
    int objectsToPlace = 20;

    IsoObjectMapInfo info = new IsoObjectMapInfo(0, 0);

    for (int i = 0; i < objectsToPlace; i++)
    {      
      int x = Random.Range(0, GlobalConstants.MapWidth);
      int y = Random.Range(0, GlobalConstants.MapHeight);

      if (_map[x, y] != 1)
      {
        _map[x, y] = 1;

        info.Init(x, y);

        GameObject go = (GameObject)Instantiate(ObjectColumn, info.WorldPosition, Quaternion.identity);
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        go.transform.localPosition = info.WorldPosition;
        go.transform.SetParent(ObjectsHolder);
        go.GetComponent<SpriteRenderer>().sortingOrder = info.SortingOrder;

        //Debug.Log(string.Format ("{0} ; {1} -> {2} {3}", x, y, info.SortingOrder, go.GetComponent<SpriteRenderer>().sortingOrder));
      }
    }
  }

  void PlaceActor()
  {    
    IsoObjectMapInfo info = new IsoObjectMapInfo(2, 1);
    Actor actor = (Actor)Instantiate(ActorObject, info.WorldPosition, Quaternion.identity);
    actor.transform.localPosition = info.WorldPosition;
    actor.transform.SetParent(ObjectsHolder);
  }
}
