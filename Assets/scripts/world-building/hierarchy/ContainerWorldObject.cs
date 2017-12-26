using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerWorldObject : WorldObjectBase 
{
  public Animation AnimationComponent;

  public float AnimationSpeed = 1.0f;

  void Awake()
  {
    AnimationComponent["Open"].speed = AnimationSpeed;   
  }
}
