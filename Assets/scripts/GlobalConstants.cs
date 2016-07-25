using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GlobalConstants
{
  public static int MapWidth = 10;
  public static int MapHeight = 10;
  public static int TileWidth = 128;
  public static int TileHeight = 64;

  // Isometric sprite is actually just a projected square,
  // so the length of its side after reverse projection
  // will be sqrt((TileWidth / 2)^2 + ((TileWidth / 2)^2)
  //
  // We take isometric sprite, double its height to get square
  // and then rotate it 45 degrees to arrive at simple square grid.
  // To get array coordinates from world ones we need to double the y
  // and then make d * Cos(45) and Sin accordingly.
  // The length of d is given below.
  // Notice the reduction of * 2 and / 2 in the second summand.
  //public static float ProjectedTileSideLength = Mathf.Sqrt(Mathf.Pow((float)TileWidth / 2.0f, 2.0f) + Mathf.Pow((float)TileHeight, 2.0f));
}
