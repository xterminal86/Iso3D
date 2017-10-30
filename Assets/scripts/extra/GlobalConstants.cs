using UnityEngine;
using System.Collections.Generic;

public delegate void Callback();
public delegate void CallbackO(object sender);
public delegate void CallbackB(bool arg);

public static class GlobalConstants 
{ 
  public const float ScaleFactor = 2.0f;
  public const float HeroMoveSpeed = 2.0f;
}



