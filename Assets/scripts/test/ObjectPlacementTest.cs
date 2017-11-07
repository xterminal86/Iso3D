using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacementTest : MonoBehaviour 
{
  public GameObject Object;
  public GameObject Cursor;
  public GameObject EditorGrid;

  float _gridSize = 0.5f;

  Vector3 _mousePos = Vector3.zero;
  Vector3 _worldPos = Vector3.zero;
  Vector3 _oldWorldPos = Vector3.zero;

  int _mapSize = 10;

  void Awake()
  {
    for (int x = -_mapSize + 1; x < _mapSize; x++)
    {
      for (int y = -_mapSize + 1; y < _mapSize; y++)
      {
        Instantiate(EditorGrid, new Vector3(x, 0.0f, y), Quaternion.identity);
      }
    }
  }

  RaycastHit _hitInfo;
  void Update()
  { 
    _mousePos = Input.mousePosition;
    _mousePos.z = Camera.main.nearClipPlane;

    Ray r = Camera.main.ScreenPointToRay(_mousePos);
    int mask = LayerMask.GetMask("EditorGrid");
    if (Physics.Raycast(r.origin, r.direction, out _hitInfo, Mathf.Infinity, mask))
    {
      Plane hPlane = new Plane(Vector3.up, Vector3.zero);
      float distance = 0; 
      Vector3 point = Vector3.zero;
      if (hPlane.Raycast(r, out distance))
      {
        point = r.GetPoint(distance);
        point.y = 0.0f;

        float d = Vector3.Distance(point, _oldWorldPos);

        if (d > _gridSize)
        {          
          float fx = point.x % _gridSize;
          float fz = point.z % _gridSize;
           
          point.x -= fx;
          point.z -= fz;

          Cursor.transform.position = point;
          _oldWorldPos = point;
        }
      }
    }

    if (Input.GetMouseButtonDown(0))
    {
      Vector3 pos = Cursor.transform.position;
      pos.y = 0.5f;

      var go = Instantiate(Object, pos, Quaternion.identity);
      go.SetActive(true);
    }
    else if (Input.GetMouseButtonDown(1))
    {
      if (Physics.BoxCast(new Vector3(Cursor.transform.position.x, -1.0f, Cursor.transform.position.z), new Vector3(0.4f, 0.4f, 0.4f), Vector3.up, out _hitInfo, Quaternion.identity, Mathf.Infinity, ~mask))
      {
        if (_hitInfo.collider.name == "object(Clone)")
        {
          Destroy(_hitInfo.collider.gameObject);
        }
      }
    }
  }
}

