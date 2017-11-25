using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraController : MonoSingleton<CameraController>
{
  public Transform CameraHolder;
  public Transform InnerCameraHolder;

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

  float _angle = 45.0f;
  float _posZ = -12.0f;
  float _cameraRotationY = 45.0f;
  float _cameraRotationSpeed = 100.0f;
  void Update()
  {
    if (Input.GetKey(KeyCode.Z))
    {
      _cameraRotationY += Time.smoothDeltaTime * _cameraRotationSpeed;
    }
    else if (Input.GetKey(KeyCode.X))
    {
      _cameraRotationY -= Time.smoothDeltaTime * _cameraRotationSpeed;
    }

    float wheel = Input.GetAxis("Mouse ScrollWheel");

    if (wheel < 0.0f)
    {
      _angle += 5.0f;
      _posZ -= 0.5f;
    }
    else if (wheel > 0.0f)
    {
      _angle -= 5.0f;
      _posZ += 0.5f;
    }

    _angle = Mathf.Clamp(_angle, 5.0f, 45.0f);
    _posZ = Mathf.Clamp(_posZ, -12.0f, -5.0f);

    Vector3 pos = InnerCameraHolder.localPosition;
    Vector3 angles = CameraHolder.eulerAngles;

    pos.z = _posZ;
    angles.x = _angle;
    angles.y = _cameraRotationY;

    InnerCameraHolder.localPosition = pos;
    CameraHolder.eulerAngles = angles;
  }
}