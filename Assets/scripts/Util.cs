using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class Util
{
  public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
  {
    Vector3 dir = point - pivot;          // get point direction relative to pivot
    dir = Quaternion.Euler(angles) * dir; // rotate it
    Vector3 newPoint = dir + pivot;       // calculate rotated point
    return newPoint;
  }  
}

public class Int2
{
  int _x = 0;
  int _y = 0;

  public Int2()
  {
    _x = 0;
    _y = 0;
  }

  public Int2(int x, int y)
  {
    _x = x;
    _y = y;
  }

  public Int2(float x, float y)
  {
    _x = (int)x;
    _y = (int)y;
  }

  public Int2(Vector2 v2)
  {
    _x = (int)v2.x;
    _y = (int)v2.y;
  }

  public int X
  {
    set { _x = value; }
    get { return _x; }
  }

  public int Y
  {
    set { _y = value; }
    get { return _y; }
  }

  public void Set(int x, int y)
  {
    X = x;
    Y = y;
  }

  public override string ToString()
  {
    return string.Format("[Int2: X={0}, Y={1}]", X, Y);
  }
}

public class IsoObjectMapInfo
{
  public int SortingOrder = 0;
  public Vector3 WorldPosition = Vector3.zero;

  public IsoObjectMapInfo(int arrayMapX, int arrayMapY)
  {    
    Init(arrayMapX, arrayMapY);
  }

  public void Init(int arrayMapX, int arrayMapY)
  {
    float tileX = (arrayMapX * -0.5f) + (arrayMapY * 0.5f);
    float tileY = (arrayMapX * -0.25f) + (arrayMapY * -0.25f);

    WorldPosition.x = tileX;
    WorldPosition.y = tileY;

    SortingOrder = (arrayMapX * 10 + arrayMapY);
  }

  Int2 _res = new Int2();
  public Int2 WorldToArrayCoords(Vector3 worldPos)
  {   
    float x = worldPos.x - 0.5f;
    float y = worldPos.y * 2.0f;

    float nx = x * Mathf.Cos(45.0f * Mathf.Deg2Rad) - y * Mathf.Sin(45.0f * Mathf.Deg2Rad);
    float ny = x * Mathf.Sin(45.0f * Mathf.Deg2Rad) + y * Mathf.Cos(45.0f * Mathf.Deg2Rad);

    int arrayX = (int)(ny / GlobalConstants.ProjectedSquareSideLength);
    int arrayY = (int)(nx / GlobalConstants.ProjectedSquareSideLength);

    _res.X = Mathf.Abs(arrayX);
    _res.Y = Mathf.Abs(arrayY);

    return _res;
  }
};