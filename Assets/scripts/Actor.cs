using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Actor : MonoBehaviour 
{	
  public SpriteRenderer SpriteRendererComponent;

  IsoObjectMapInfo _info = new IsoObjectMapInfo(0, 0);

  Vector3 _worldPosition = Vector3.zero;
  Vector3 _cameraPosition = new Vector3(0.0f, 0.0f, -10.0f);

  Int2 _mapPosition = new Int2();
  float _moveSpeed = 1.0f;
  void Update()
  {
    _worldPosition = transform.localPosition;

    if (Input.GetKey(KeyCode.W))
    { 
      _worldPosition.x += (_moveSpeed * 2) * Time.smoothDeltaTime;
      _worldPosition.y += _moveSpeed * Time.smoothDeltaTime;
    }
    else if (Input.GetKey(KeyCode.S))
    {
      _worldPosition.x -= (_moveSpeed * 2) * Time.smoothDeltaTime;
      _worldPosition.y -= _moveSpeed * Time.smoothDeltaTime;
    }
    else if (Input.GetKey(KeyCode.D))
    {
      _worldPosition.x += (_moveSpeed * 2) * Time.smoothDeltaTime;
      _worldPosition.y -= _moveSpeed * Time.smoothDeltaTime;
    }
    else if (Input.GetKey(KeyCode.A))
    {
      _worldPosition.x -= (_moveSpeed * 2) * Time.smoothDeltaTime;
      _worldPosition.y += _moveSpeed * Time.smoothDeltaTime;
    }

    if (Input.GetMouseButtonDown(0))
    {
      Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

      Int2 arrayCoords = _info.WorldToArrayCoords(worldPos);
      _info.Init(arrayCoords.X, arrayCoords.Y);

      Debug.Log (worldPos + " -> " + arrayCoords + " " + _info.SortingOrder);
    }
      
    _mapPosition = _info.WorldToArrayCoords(_worldPosition);
    _info.Init(_mapPosition.X, _mapPosition.Y);

    SpriteRendererComponent.sortingOrder = _info.SortingOrder;

    transform.localPosition = _worldPosition;

    _cameraPosition.x = _worldPosition.x;
    _cameraPosition.y = _worldPosition.y;

    Camera.main.transform.position = _cameraPosition;
  }
}
