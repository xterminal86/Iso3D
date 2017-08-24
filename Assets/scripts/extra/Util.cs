using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

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
  
  public override string ToString()
  {
    return string.Format("[Int2: X={0}, Y={1}]", X, Y);
  }
}

public class RoomBounds
{
  public Int2 FirstPoint = new Int2();
  public Int2 SecondPoint = new Int2();
  
  public RoomBounds(Int2 p1, Int2 p2)
  {
    FirstPoint.X = p1.X;
    FirstPoint.Y = p1.Y;
    
    SecondPoint.X = p2.X;
    SecondPoint.Y = p2.Y;
  }
  
  public override string ToString()
  {
    return string.Format("[RoomBounds] -> [" + FirstPoint + " " + SecondPoint + "]");
  }
}

public class Int3
{
  int _x = 0;
  int _y = 0;
  int _z = 0;

  public static Int3 Zero
  {
    get { return new Int3(); }
  }

  public Int3()
  {
    _x = 0;
    _y = 0;
    _z = 0;
  }

  public Int3(int x, int y, int z)
  {
    _x = x;
    _y = y;
    _z = z;
  }

  public Int3(float x, float y, float z)
  {
    _x = (int)x;
    _y = (int)y;
    _z = (int)z;
  }

  public Int3(Vector3 v3)
  {
    _x = (int)v3.x;
    _y = (int)v3.y;
    _z = (int)v3.z;
  }

  public Int3(Int3 v3)
  {
    _x = v3.X;
    _y = v3.Y;
    _z = v3.Z;
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

  public int Z
  {
    set { _z = value; }
    get { return _z; }
  }

  public void Set(float x, float y, float z)
  {
    _x = (int)x;
    _y = (int)y;
    _z = (int)z;
  }

  public void Set(int x, int y, int z)
  {
    _x = x;
    _y = y;
    _z = z;
  }

  public void Set(Int3 arg)
  {
    _x = arg.X;
    _y = arg.Y;
    _z = arg.Z;
  }

  public void Set(Vector3 arg)
  {
    _x = (int)arg.x;
    _y = (int)arg.y;
    _z = (int)arg.z;
  }

  public static bool operator ==(Int3 a, Int3 b)
  {
    return (a.X == b.X && a.Y == b.Y && a.Z == b.Z);
  }

  public static bool operator !=(Int3 a, Int3 b)
  {
    return (a.X != b.X || a.Y != b.Y || a.Z != b.Z);
  }

  public override string ToString()
  {
    return string.Format("[Int3: X={0}, Y={1}, Z={2}]", X, Y, Z);
  }
}
