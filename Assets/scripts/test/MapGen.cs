using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour 
{
  public uint MapWidth = 0;
  public uint MapHeight = 0;

  public bool UseRandomSeed = false;
  public string RandomSeed = string.Empty;

  [Range(0, 100)]
  public int FillPercent = 0;

  public bool DrawGizmos = true;

  int[,] _map;

  int _smoothSteps = 5;
  void Start()
  {
    GenerateMap();
  }

  void SmoothMap()
  {
    for (int x = 0; x < MapWidth; x++)
    {
      for (int y = 0; y < MapHeight; y++)
      {
        int neighbours = GetNeighbours(x, y);

        if (neighbours > 4)
        {
          _map[x, y] = 1;
        }
        else if (neighbours < 4)
        {
          _map[x, y] = 0;
        }
      }
    }
  }

  int GetNeighbours(int x, int y)
  {
    int wallCount = 0;

    for (int nx = x - 1; nx <= x + 1; nx++)
    {
      for (int ny = y - 1; ny <= y + 1; ny++)
      {
        if (nx == x && ny == y)
        {
          continue;
        }

        if (nx >= 0 && nx < MapWidth && ny >= 0 && ny < MapHeight)
        {
          wallCount += _map[nx, ny];
        }
        else
        {
          wallCount++;
        }
      }
    }

    return wallCount;
  }

  void GenerateMap()
  {
    _map = new int[MapWidth, MapHeight];

    if (UseRandomSeed)
    {
      RandomSeed = Time.time.ToString();
    }

    System.Random rng = new System.Random(RandomSeed.GetHashCode());

    for (int x = 0; x < MapWidth; x++)
    {
      for (int y = 0; y < MapHeight; y++)
      {
        if (x == 0 || x == MapWidth - 1 || y == 0 || y == MapHeight - 1)
        {
          _map[x, y] = 1;
        }
        else
        {
          _map[x, y] = (rng.Next(0, 100) < FillPercent) ? 1 : 0;
        }
      }
    }

    for (int i = 0; i < _smoothSteps; i++)
    {
      SmoothMap();
    }

    MeshGenerator mg = GetComponent<MeshGenerator>();
    if (mg != null)
    {
      mg.GenerateMesh(_map, 1);
    }
  }

  void OnDrawGizmos()
  {
    if (DrawGizmos)
    {
      if (_map != null)
      {
        for (int x = 0; x < MapWidth; x++)
        {
          for (int y = 0; y < MapHeight; y++)
          {
            Gizmos.color = (_map[x, y] == 1) ? Color.black : Color.white;
            Vector3 pos = new Vector3(-MapWidth / 2.0f + x + 0.5f, 0, -MapHeight / 2.0f + y + 0.5f);
            Gizmos.DrawCube(pos, Vector3.one);
          }
        }
      }
    }
  }

  void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      GenerateMap();
    }
  }
}
