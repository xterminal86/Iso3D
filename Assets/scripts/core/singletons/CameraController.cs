using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraController : MonoSingleton<CameraController>
{
  public GameObject MouseMap;

  Vector3 _mouseMapPosition = Vector3.zero;
  GameObject _mouseMap;
  public void SetupCamera(Vector3 playerPos)
  {
    _mouseMap = Instantiate(MouseMap);

    UpdateCameraPosition(playerPos);
  }

  public void UpdateCameraPosition(Vector3 newPosition)
  {
    _mouseMapPosition = newPosition;
    _mouseMap.transform.position = _mouseMapPosition;
    transform.position = newPosition;
  }
}