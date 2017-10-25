using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraController : MonoSingleton<CameraController>
{
  public void SetupCamera(Int3 playerPos)
  {    
    _heroRef = GameObject.Find("hero").GetComponent<HeroController>();

    _heroRef.RigidbodyComponent.position = new Vector3(playerPos.X, playerPos.Y, playerPos.Z);

    _heroRef.gameObject.SetActive(true);
  }

  HeroController _heroRef;
  public void LockOnHero(HeroController hero)
  {
    _heroRef = hero;
  }

  void Update()
  {
    if (_heroRef != null)
    {
      transform.position = _heroRef.transform.position;
    }
  }
}