using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleObstacle : WorldObjectBase
{
  public Renderer RendererComponent;

  public override void Init(SerializedWorldObject serializedObject)
  {
    SerializedObject = serializedObject;
  }

  public override void PostProcess()
  {
    RendererComponent.enabled = false;

    //Color c = new Color(0, 0, 0, 0);
    //RendererComponent.material.color = c;
  }
}
