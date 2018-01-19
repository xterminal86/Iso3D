using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerRing : MonoBehaviour 
{
  public float ScaledownSpeed = 0.5f;

  void Update()
  {
    if (transform.localScale.x < 0.0f)
    {
      Destroy(gameObject);
    }
    else
    {
      Vector3 scale = transform.localScale;

      scale.x -= Time.smoothDeltaTime * ScaledownSpeed;
      scale.y -= Time.smoothDeltaTime * ScaledownSpeed;
      scale.z -= Time.smoothDeltaTime * ScaledownSpeed;

      transform.localScale = scale;
    }
  }
}
