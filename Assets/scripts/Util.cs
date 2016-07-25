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

    SortingOrder = (arrayMapX * 10 + arrayMapY + 1);
  }

  Int2 _res = new Int2();
  public Int2 WorldToArrayCoords(Vector3 worldPos)
  {   
    float doubledY = worldPos.y * 2.0f;

    float nx = worldPos.x * Mathf.Cos(45.0f * Mathf.Deg2Rad);
    float ny = doubledY * Mathf.Sin(45.0f * Mathf.Deg2Rad);

    int arrayX = (int)(nx);
    int arrayY = (int)(ny);

    Debug.Log(string.Format("{0} -> ({1} ; {2}) = [{3} ; {4}]", worldPos, nx, ny, arrayX, arrayY));

    return _res;
  }
};