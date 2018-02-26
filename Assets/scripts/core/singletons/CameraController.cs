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

  [Range(-30.0f, -5.0f)]    
  public float CameraDistanceToChar = -20.0f;

  void Awake()
  {
    Vector3 pos = InnerCameraHolder.localPosition;
    pos.z = CameraDistanceToChar;
    InnerCameraHolder.localPosition = pos;
  }

  Vector3 _mouseMapPosition = Vector3.zero;
  GameObject _mouseMap;
  HeroController3D _heroControllerRef;
  public void SetupCamera(Vector3 playerPos, HeroController3D heroControllerRef)
  {
    _heroControllerRef = heroControllerRef;

    _mouseMap = Instantiate(MouseMap);

    //UpdateCameraPosition(playerPos);
  }

  public void UpdateCameraPosition(Vector3 newPosition)
  {
    _mouseMapPosition = newPosition;
    _mouseMap.transform.position = _mouseMapPosition;
    transform.position = newPosition;
  }

  float _angle = 45.0f;
  float _posZ = -20.0f;
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

    Vector3 angles = CameraHolder.eulerAngles;

    angles.x = _angle;
    angles.y = _cameraRotationY;

    CameraHolder.eulerAngles = angles;

    if (_heroControllerRef != null)
    {
      UpdateCameraPosition(_heroControllerRef.RigidbodyComponent.position);
    }
  }
}