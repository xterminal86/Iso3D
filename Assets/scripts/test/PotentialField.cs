using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotentialField : MonoBehaviour 
{
  public Transform Holder;

  public GameObject PathfindingCellPrefab;
  public GameObject Actor;

  const int MapSize = 16;

  MapObject[,] _map = new MapObject[MapSize, MapSize];

  PathfindingCell[,] _pathfindingMap = new PathfindingCell[MapSize, MapSize];

  void Start()
  {
    for (int x = 0; x < MapSize; x++)
    { 
      for (int y = 0; y < MapSize; y++)
      {
        _map[x, y] = new MapObject();
        _map[x, y].Coords = new Int2(x, y);

        GameObject go = Instantiate(PathfindingCellPrefab, new Vector3(x, y, 0.0f), Quaternion.identity, Holder);
        _pathfindingMap[x, y] = go.GetComponent<PathfindingCell>();

        _pathfindingMap[x, y].SetMapOverlay(".");
      }
    }

    PlaceObstacle(0, 2);
    PlaceObstacle(1, 2);
    PlaceObstacle(0, 4);
    PlaceObstacle(1, 4);
    PlaceObstacle(1, 5);
    PlaceObstacle(0, 6);
    PlaceObstacle(1, 6);
    PlaceObstacle(2, 2);
    PlaceObstacle(3, 4);
    PlaceObstacle(4, 0);
    PlaceObstacle(4, 1);
    PlaceObstacle(4, 2);
    PlaceObstacle(4, 4);
    PlaceObstacle(5, 4);
    PlaceObstacle(6, 2);
    PlaceObstacle(6, 3);
    PlaceObstacle(6, 4);

    for (int x = 2; x < MapSize - 1; x++)
    {
      PlaceObstacle(x, 6);
    }

    PlaceObstacle(8, 1);
    PlaceObstacle(8, 2);
    PlaceObstacle(8, 3);
    PlaceObstacle(8, 4);
    PlaceObstacle(8, 5);

    PlaceTarget(MapSize - 2, MapSize - 2);

    RefreshMap();

    PlaceActor(0, 0);
  }

  void PlaceActor(int x, int y)
  {
    Vector3 pos = new Vector3(x, y, 0.0f);
    Actor.transform.position = pos;

    StartCoroutine(FindPathRoutine());
  }

  IEnumerator FindPathRoutine()
  {
    while ((int)Actor.transform.position.x != _targetPos.X
        || (int)Actor.transform.position.y != _targetPos.Y)
    {
      Int2 currentPos = new Int2(Actor.transform.position.x, Actor.transform.position.y);

      int currentWeight = _map[currentPos.X, currentPos.Y].Weight;

      Int2 newPos = FindNextCell(currentPos, currentWeight);
      if (newPos != null)
      {  
        Vector3 pos = new Vector3(newPos.X, newPos.Y, 0.0f);
        Actor.transform.position = pos;

        float timer = 0.0f;
        while (timer < 0.5f)
        {
          timer += Time.smoothDeltaTime;

          yield return null;
        }
      }
      else
      {
        yield break;
      }
    }

    yield return null;
  }

  Int2 FindNextCell(Int2 pos, int weight)
  {
    int lx = pos.X - 1;
    int ly = pos.Y - 1;
    int hx = pos.X + 1;
    int hy = pos.Y + 1;

    int w = weight;
    Int2 newPos = new Int2(pos);

    for (int x = lx; x <= hx; x++)
    {
      for (int y = ly; y <= hy; y++)
      {
        if (x < 0 || x > MapSize - 1 || y < 0 || y > MapSize - 1)
        {
          continue;
        }

        if (x == pos.X && y == pos.Y)
        {
          continue;
        }

        if (_map[x, y].Weight >= 0 && _map[x, y].Weight < w)
        {        
          w = _map[x, y].Weight;
          newPos.Set(x, y);
        }
      }
    }

    if (w != weight)
    {
      return newPos;
    }

    return null;
  }

  void RefreshMap()
  {
    for (int x = 0; x < MapSize; x++)
    { 
      for (int y = 0; y < MapSize; y++)
      {
        _pathfindingMap[x, y].SetPathOverlay(_map[x, y].Weight.ToString());
      }
    }
  }

  void PlaceObstacle(int x, int y)
  {
    _map[x, y].IsObstacle = true;
    _pathfindingMap[x, y].SetMapOverlay("#");
  }

  Int2 _targetPos = new Int2();
  void PlaceTarget(int x, int y)
  {
    _targetPos.Set(x, y);

    _map[x, y].IsTarget = true;

    _pathfindingMap[x, y].SetMapOverlay("X");

    GenerateField(_map[x, y]);
  }

  Queue<MapObject> _markedCells = new Queue<MapObject>();
  void GenerateField(MapObject target)
  {
    target.Weight = 0;
    target.IsMarked = true;
    _markedCells.Enqueue(target);

    int safeguard = 0;
    while (_markedCells.Count != 0)
    {
      if (safeguard > 1000)
      {
        Debug.LogWarning("Terminated by safeguard!");
        break;
      }

      var mo = _markedCells.Dequeue();

      Int2 queuedPos = mo.Coords;

      int lx = queuedPos.X - 1;
      int hx = queuedPos.X + 1;
      int ly = queuedPos.Y - 1;
      int hy = queuedPos.Y + 1;

      AddNeighbour(new Int2(lx, queuedPos.Y), _map[queuedPos.X, queuedPos.Y].Weight);
      AddNeighbour(new Int2(hx, queuedPos.Y), _map[queuedPos.X, queuedPos.Y].Weight);
      AddNeighbour(new Int2(queuedPos.X, ly), _map[queuedPos.X, queuedPos.Y].Weight);
      AddNeighbour(new Int2(queuedPos.X, hy), _map[queuedPos.X, queuedPos.Y].Weight);

      safeguard++;
    }
      
    /*
    for (int x = 0; x < MapSize; x++)
    { 
      for (int y = 0; y < MapSize; y++)
      {        
        if (_map[x, y].IsObstacle)
        {
          _map[x, y].Weight = Util.BlockDistance(pos, new Int2(x, y)) + 500;
        }
        else
        {
          _map[x, y].Weight = Util.BlockDistance(pos, new Int2(x, y));
        }
      }
    }
    */
  }

  void AddNeighbour(Int2 pos, int prevWeight)
  {
    if (pos.X >= 0 && pos.Y >= 0 && pos.X < MapSize && pos.Y < MapSize && !_map[pos.X, pos.Y].IsMarked && !_map[pos.X, pos.Y].IsObstacle)
    {       
      _map[pos.X, pos.Y].Weight = prevWeight + 1;
      _map[pos.X, pos.Y].IsMarked = true;
      _markedCells.Enqueue(_map[pos.X, pos.Y]);
    }
  }

  class MapObject
  {
    public int Weight = -1;
    public bool IsObstacle = false;
    public bool IsTarget = false;
    public bool IsMarked = false;
    public Int2 Coords = new Int2();
  }
}
