using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectBase : MonoBehaviour 
{
  [HideInInspector]
  public string PrefabName = string.Empty;

  [HideInInspector]
  public Vector3 WorldPosition = Vector3.zero;

  [HideInInspector]
  public float RotationAngle = 0.0f;

  // SerializedObject is populated by game editor. Specific instance is created in Awake() method of
  // appropriate objects

  [HideInInspector]
  public SerializedWorldObject SerializedObject = new SerializedWorldObject();

  /// <summary>
  /// Cleanup after deselecting object in game editor
  /// </summary>
  public virtual void Deselect()
  {
  }

  /// <summary>
  /// Called during instantiation of level in actual game
  /// </summary>
  public virtual void Init(SerializedWorldObject serializedObject)
  {
  }

  /// <summary>
  /// Do something else after all initialization has completed
  /// </summary>
  public virtual void PostProcess()
  {
  }
}
