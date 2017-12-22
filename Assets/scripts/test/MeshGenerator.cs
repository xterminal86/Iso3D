using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://github.com/SebLague/Procedural-Cave-Generation

public class MeshGenerator : MonoBehaviour 
{
  public Texture2D Bitmap;

  public MeshFilter Walls;

  public bool DrawGizmos = true;

  public SquareGrid SquareGridObject;

  Dictionary<int, List<Triangle>> _trianglesByVertex = new Dictionary<int, List<Triangle>>();

  List<List<int>> _outlines = new List<List<int>>();
  HashSet<int> _checkedVertices = new HashSet<int>();

  public void GenerateMesh(int[,] map, float squareSize)
  {    
    _trianglesByVertex.Clear();
    _outlines.Clear();
    _checkedVertices.Clear();

    if (Bitmap == null)
    {
      GenerateMap(map, squareSize); 
    }
    else
    {
      GenerateMapFromBitmap(squareSize);
    }

    CreateWallMesh();
  }

  void CreateWallMesh()
  {
    CalculateMeshOutlines();

    List<Vector3> wallVertices = new List<Vector3>();
    List<int> wallTriangles = new List<int>();

    Mesh wallMesh = new Mesh();
    float wallHeight = 5.0f;

    foreach (var outline in _outlines)
    {
      for (int i = 0; i < outline.Count - 1; i++)
      {
        int startIndex = wallVertices.Count;

        wallVertices.Add(_vertices[outline[i]]);                                // Left
        wallVertices.Add(_vertices[outline[i + 1]]);                            // Right
        wallVertices.Add(_vertices[outline[i]] - Vector3.up * wallHeight);      // Bottom Left
        wallVertices.Add(_vertices[outline[i + 1]] - Vector3.up * wallHeight);  // Bottom Right

        wallTriangles.Add(startIndex + 0);
        wallTriangles.Add(startIndex + 2);
        wallTriangles.Add(startIndex + 3);

        wallTriangles.Add(startIndex + 3);
        wallTriangles.Add(startIndex + 1);
        wallTriangles.Add(startIndex + 0);
      }
    }

    wallMesh.vertices = wallVertices.ToArray();
    wallMesh.triangles = wallTriangles.ToArray();
    Walls.mesh = wallMesh;
    Walls.mesh.RecalculateNormals();
  }

  int[,] _map;
  void GenerateMapFromBitmap(float squareSize)
  {
    _map = new int[Bitmap.width, Bitmap.height];

    for (int x = 0; x < Bitmap.width; x++)
    {
      for (int y = 0; y < Bitmap.height; y++)
      {
        Color c = Bitmap.GetPixel(x, y);

        _map[x, y] = (c.r == 1.0f && c.g == 1.0f && c.b == 1.0f) ? 1 : 0;
      }
    }

    GenerateMap(_map, squareSize);
  }

  void GenerateMap(int[,] map, float squareSize)
  {
    _vertices.Clear();
    _triangles.Clear();

    SquareGridObject = new SquareGrid(map, squareSize);

    for (int x = 0; x < SquareGridObject.Squares.GetLength(0); x++)
    {
      for (int y = 0; y < SquareGridObject.Squares.GetLength(1); y++)
      {
        TriangulateSquare(SquareGridObject.Squares[x, y]);
      }
    }

    Mesh mesh = new Mesh();
    GetComponent<MeshFilter>().mesh = mesh;
    mesh.vertices = _vertices.ToArray();
    mesh.triangles = _triangles.ToArray();
    mesh.RecalculateNormals();

    int tileAmount = 10;

    Vector2[] uvs = new Vector2[_vertices.Count];
    for (int i = 0; i < _vertices.Count; i++)
    {
      float percentX = Mathf.InverseLerp(-map.GetLength(0) / 2 * squareSize, map.GetLength(0) / 2 * squareSize, _vertices[i].x) * tileAmount;
      float percentY = Mathf.InverseLerp(-map.GetLength(1) / 2 * squareSize, map.GetLength(1) / 2 * squareSize, _vertices[i].z) * tileAmount;

      uvs[i] = new Vector2(percentX, percentY);
    }

    mesh.uv = uvs;
  }

  void TriangulateSquare(Square square)
  {
    switch (square.Configuration)
    {
      case 0:
        break;

        // 1 point

      case 1:
        MeshFromPoints(square.CenterLeft, square.CenterBottom, square.BottomLeft);
        break;

      case 2:
        MeshFromPoints(square.BottomRight, square.CenterBottom, square.CenterRight);
        break;

      case 4:
        MeshFromPoints(square.TopRight, square.CenterRight, square.CenterTop);
        break;

      case 8:
        MeshFromPoints(square.TopLeft, square.CenterTop, square.CenterLeft);
        break;

        // 2 points

      case 3:
        MeshFromPoints(square.CenterRight, square.BottomRight, square.BottomLeft, square.CenterLeft);
        break;

      case 6:
        MeshFromPoints(square.CenterTop, square.TopRight, square.BottomRight, square.CenterBottom);
        break;

      case 9:
        MeshFromPoints(square.TopLeft, square.CenterTop, square.CenterBottom, square.BottomLeft);
        break;

      case 12:
        MeshFromPoints(square.TopLeft, square.TopRight, square.CenterRight, square.CenterLeft);
        break;

        // diagonal points

      case 5:
        MeshFromPoints(square.CenterTop, square.TopRight, square.CenterRight, square.CenterBottom, square.BottomLeft, square.CenterLeft);
        break;

      case 10:
        MeshFromPoints(square.TopLeft, square.CenterTop, square.CenterRight, square.BottomRight, square.CenterBottom, square.CenterLeft);
        break;

        // 3 points

      case 7:
        MeshFromPoints(square.CenterTop, square.TopRight, square.BottomRight, square.BottomLeft, square.CenterLeft);
        break;

      case 11:
        MeshFromPoints(square.TopLeft, square.CenterTop, square.CenterRight, square.BottomRight, square.BottomLeft);
        break;

      case 13:
        MeshFromPoints(square.TopLeft, square.TopRight, square.CenterRight, square.CenterBottom, square.BottomLeft);
        break;

      case 14:
        MeshFromPoints(square.TopLeft, square.TopRight, square.BottomRight, square.CenterBottom, square.CenterLeft);
        break;

        // 4 points

      case 15:
        MeshFromPoints(square.TopLeft, square.TopRight, square.BottomRight, square.BottomLeft);
        _checkedVertices.Add(square.TopLeft.MeshIndex);
        _checkedVertices.Add(square.TopRight.MeshIndex);
        _checkedVertices.Add(square.BottomRight.MeshIndex);
        _checkedVertices.Add(square.BottomLeft.MeshIndex);
        break;
    }
  }

  List<Vector3> _vertices = new List<Vector3>();
  List<int> _triangles = new List<int>();

  void MeshFromPoints(params Node[] points)
  {
    AssignVertices(points);

    if (points.Length >= 3)
    {
      CreateTriangle(points[0], points[1], points[2]);
    }

    if (points.Length >= 4)
    {
      CreateTriangle(points[0], points[2], points[3]);
    }

    if (points.Length >= 5)
    {
      CreateTriangle(points[0], points[3], points[4]);
    }

    if (points.Length >= 6)
    {
      CreateTriangle(points[0], points[4], points[5]);
    }
  }

  void CreateTriangle(Node a, Node b, Node c)
  {
    _triangles.Add(a.MeshIndex);
    _triangles.Add(b.MeshIndex);
    _triangles.Add(c.MeshIndex);

    Triangle t = new Triangle(a.MeshIndex, b.MeshIndex, c.MeshIndex);
    AddTriangleToDictionary(t.VertexIndexA, t);
    AddTriangleToDictionary(t.VertexIndexB, t);
    AddTriangleToDictionary(t.VertexIndexC, t);
  }

  void CalculateMeshOutlines()
  {
    for (int i = 0; i < _vertices.Count; i++)
    {
      if (!_checkedVertices.Contains(i))
      {
        int newOutlineVertex = GetConnectedOutlineVertex(i);
        if (newOutlineVertex != -1)
        {
          _checkedVertices.Add(i);

          List<int> outline = new List<int>() { i };
          _outlines.Add(outline);
          FollowOutline(newOutlineVertex, _outlines.Count - 1);
          _outlines[_outlines.Count - 1].Add(i);
        }
      }
    }
  }

  void FollowOutline(int vertexIndex, int outlineIndex)
  {
    _outlines[outlineIndex].Add(vertexIndex);
    _checkedVertices.Add(vertexIndex);
    int nextVertexIndex = GetConnectedOutlineVertex(vertexIndex);

    if (nextVertexIndex != -1)
    {
      FollowOutline(nextVertexIndex, outlineIndex);
    }
  }

  int GetConnectedOutlineVertex(int vertexIndex)
  {
    List<Triangle> t = _trianglesByVertex[vertexIndex];

    foreach (var item in t)
    {
      for (int i = 0; i < 3; i++)
      {
        int vertexB = item[i];

        if (vertexB != vertexIndex && !_checkedVertices.Contains(vertexB))
        {
          if (IsOutlineEdge(vertexIndex, vertexB))
          {
            return vertexB;
          }
        }
      }
    }

    return -1;
  }

  bool IsOutlineEdge(int vertexIndexA, int vertexIndexB)
  {
    int sharedTriangleCount = 0;
    var trianglesContainingVertexA = _trianglesByVertex[vertexIndexA];
    foreach (var item in trianglesContainingVertexA)
    {
      if (item.Contains(vertexIndexB))
      {
        sharedTriangleCount++;

        if (sharedTriangleCount > 1)
        {
          break;
        }
      }
    }

    return (sharedTriangleCount == 1);
  }

  void AddTriangleToDictionary(int vertexIndex, Triangle t)
  {
    if (_trianglesByVertex.ContainsKey(vertexIndex))
    {
      _trianglesByVertex[vertexIndex].Add(t);
    }
    else
    {
      List<Triangle> list = new List<Triangle>() { t };
      _trianglesByVertex.Add(vertexIndex, list);
    }
  }

  struct Triangle
  {
    public int VertexIndexA;
    public int VertexIndexB;
    public int VertexIndexC;

    int[] _vertices;

    public Triangle(int va, int vb, int vc)
    {      
      VertexIndexA = va;
      VertexIndexB = vb;
      VertexIndexC = vc;

      _vertices = new int[3];

      _vertices[0] = va;
      _vertices[1] = vb;
      _vertices[2] = vc;
    }

    public bool Contains(int vertexIndex)
    {
      return (vertexIndex == VertexIndexA || vertexIndex == VertexIndexB || vertexIndex == VertexIndexC);
    }

    public int this[int i]
    {
      get { return _vertices[i]; } 
    }
  }

  void AssignVertices(Node[] points)
  {
    for (int i = 0; i < points.Length; i++)
    {
      if (points[i].MeshIndex == -1)
      {
        points[i].MeshIndex = _vertices.Count;
        _vertices.Add(points[i].Position);
      }
    }
  }

  void OnDrawGizmos()
  {
    if (DrawGizmos)
    {
      if (SquareGridObject != null)
      {
        for (int x = 0; x < SquareGridObject.Squares.GetLength(0); x++)
        {
          for (int y = 0; y < SquareGridObject.Squares.GetLength(1); y++)
          {
            Gizmos.color = SquareGridObject.Squares[x, y].TopLeft.IsActive ? Color.black : Color.white;
            Gizmos.DrawCube(SquareGridObject.Squares[x, y].TopLeft.Position, Vector3.one * 0.4f);

            Gizmos.color = SquareGridObject.Squares[x, y].TopRight.IsActive ? Color.black : Color.white;
            Gizmos.DrawCube(SquareGridObject.Squares[x, y].TopRight.Position, Vector3.one * 0.4f);

            Gizmos.color = SquareGridObject.Squares[x, y].BottomRight.IsActive ? Color.black : Color.white;
            Gizmos.DrawCube(SquareGridObject.Squares[x, y].BottomRight.Position, Vector3.one * 0.4f);

            Gizmos.color = SquareGridObject.Squares[x, y].BottomLeft.IsActive ? Color.black : Color.white;
            Gizmos.DrawCube(SquareGridObject.Squares[x, y].BottomLeft.Position, Vector3.one * 0.4f);

            Gizmos.color = Color.gray;

            Gizmos.DrawCube(SquareGridObject.Squares[x, y].CenterTop.Position, Vector3.one * 0.15f);
            Gizmos.DrawCube(SquareGridObject.Squares[x, y].CenterRight.Position, Vector3.one * 0.15f);
            Gizmos.DrawCube(SquareGridObject.Squares[x, y].CenterBottom.Position, Vector3.one * 0.15f);
            Gizmos.DrawCube(SquareGridObject.Squares[x, y].CenterLeft.Position, Vector3.one * 0.15f);
          }
        }
      }
    }
  }

  public class SquareGrid
  {
    public Square[,] Squares;

    public SquareGrid(int[,] map, float squareSize)
    {      
      int nodeCountX = map.GetLength(0);
      int nodeCountY = map.GetLength(1);

      float mapWidth = nodeCountX * squareSize;
      float mapHeight = nodeCountY * squareSize;

      ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];

      for (int x = 0; x < nodeCountX; x++)
      {
        for (int y = 0; y < nodeCountY; y++)
        {
          Vector3 pos = new Vector3(-mapWidth / 2.0f + x * squareSize + squareSize / 2.0f, 0.0f, -mapHeight / 2.0f + y * squareSize + squareSize / 2.0f);
          controlNodes[x, y] = new ControlNode(pos, map[x, y] == 1, squareSize);
        }
      }

      Squares = new Square[nodeCountX - 1, nodeCountY - 1];

      for (int x = 0; x < nodeCountX - 1; x++)
      {
        for (int y = 0; y < nodeCountY - 1; y++)
        {
          Squares[x, y] = new Square(controlNodes[x, y + 1], controlNodes[x + 1, y + 1], controlNodes[x + 1, y], controlNodes[x, y]);
        }
      }
    }
  };

  public class Square
  {
    public ControlNode TopLeft, TopRight, BottomRight, BottomLeft;
    public Node CenterTop, CenterRight, CenterBottom, CenterLeft;

    // Each ControlNode status is coded into 4 bit integer, clockwise order, i.e.
    // 
    // TL  ct   TR
    //      
    // cl       cr
    //
    // BL  cb   BR
    //
    // If we have TopLeft and BottomRight on, it would be 1010 = 10
    // And so on and so forth.

    public int Configuration = 0;

    public Square(ControlNode topLeft, ControlNode topRight, ControlNode bottomRight, ControlNode bottomLeft)
    {      
      TopLeft = topLeft;
      TopRight = topRight;
      BottomRight = bottomRight;
      BottomLeft = bottomLeft;

      CenterTop = TopLeft.RightNode;
      CenterRight = BottomRight.AboveNode;
      CenterBottom = BottomLeft.RightNode;
      CenterLeft = BottomLeft.AboveNode;

      if (TopLeft.IsActive)     Configuration += 8;
      if (TopRight.IsActive)    Configuration += 4;
      if (BottomRight.IsActive) Configuration += 2;
      if (BottomLeft.IsActive)  Configuration += 1;
    }
  };

  public class Node
  {
    public Vector3 Position = Vector3.zero;
    public int MeshIndex = -1;

    public Node(Vector3 pos)
    {
      Position = pos;
    }
  };

  public class ControlNode : Node
  {
    public bool IsActive = false;
    public Node AboveNode;
    public Node RightNode;

    public ControlNode(Vector3 pos, bool active, float squareSize) : base(pos)
    {
      IsActive = active;
      AboveNode = new Node(Position + Vector3.forward * squareSize / 2.0f);
      RightNode = new Node(Position + Vector3.right * squareSize / 2.0f);
    }
  };
}
