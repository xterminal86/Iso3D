using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoSingleton<GameTime> 
{
  float _timer = 0.0f;
  public void SetTimer(float seconds, Callback cb)
  {
    _timer = seconds;
  }

  void Update()
  {
    if (_timer > 0.0f)
    {
      _timer -= Time.deltaTime;
    }
  }
}
