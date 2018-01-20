using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerRing : MonoBehaviour 
{
  public float ScaledownSpeed = 0.5f;
  public float RingAppearSpeed = 3.0f;

  Color _ringColor;
  Renderer _renderer;
  public void Init(Color c)
  {
    _ringColor = c;
    _renderer = GetComponentInChildren<Renderer>();
    _renderer.material.SetColor("_TintColor", _ringColor);
  }

  void Update()
  {
    if (transform.localScale.x < 0.0f)
    {
      Destroy(gameObject);
    }
    else
    {
      _ringColor.a += Time.smoothDeltaTime * RingAppearSpeed;

      _ringColor.a = Mathf.Clamp(_ringColor.a, 0.0f, 1.0f);

      _renderer.material.SetColor("_TintColor", _ringColor);
      
      Vector3 scale = transform.localScale;

      scale.x -= Time.smoothDeltaTime * ScaledownSpeed;
      scale.y -= Time.smoothDeltaTime * ScaledownSpeed;
      scale.z -= Time.smoothDeltaTime * ScaledownSpeed;

      transform.localScale = scale;
    }
  }
}
