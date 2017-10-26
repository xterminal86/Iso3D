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
  public void SetupCamera(Int3 playerPos)
  {
    _mouseMap = Instantiate(MouseMap);
    _mouseMapPosition.Set(playerPos.X, playerPos.Y, playerPos.Z);
    _mouseMap.transform.position = _mouseMapPosition;

    _heroRef = GameObject.Find("hero").GetComponent<HeroController>();

    _heroRef.RigidbodyComponent.position = new Vector3(playerPos.X, playerPos.Y, playerPos.Z);

    _heroRef.gameObject.SetActive(true);
  }

  HeroController _heroRef;
  public void LockOnHero(HeroController hero)
  {
    _heroRef = hero;
  }

  RaycastHit _hitInfo;
  void Update()
  {    
    if (_heroRef != null)
    {      
      transform.position = _heroRef.transform.position;
      _mouseMapPosition.Set(_heroRef.transform.position.x, _heroRef.transform.position.y, _heroRef.transform.position.z);
      _mouseMap.transform.position = _mouseMapPosition;
    }
  }
}