using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

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

  public GlobalConstants.InteractableObjects InteractableObjectType = GlobalConstants.InteractableObjects.NONE;

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
  /// Do something else after all initialization has completed (see InvisibleObstacle.cs)
  /// </summary>
  public virtual void PostProcess()
  {    
  }

  /// <summary>
  /// Handles monologues of characters when they are looking at object
  /// </summary>
  /// <param name="inspectingActor">Inspecting actor</param>
  public virtual void Inspect(ActorStats inspectingActor)
  {
  }

  /// <summary>
  /// Speak with "object" by selecting actor or topic in FormTalk
  /// </summary>
  /// <param name="actor">Actor who initiated dialog or null if topic is used</param>
  /// <param name="topic">Topic of conversation selected in FormTalk or null if actor was selected</param>
  public virtual void Speak(ActorStats actor, string topic)
  {
  }

  // Controller can control multiple objects (e.g. one lever opens more than one door)
  public List<Callback> Interactions = new List<Callback>();

  /// <summary>
  /// Interact with object using hand (e.g. pull lever)
  /// </summary>
  public virtual void Interact()
  {
    foreach (var item in Interactions)
    {
      if (item != null)
        item();
    }
  }

  /// <summary>
  /// _interactions are subscribed on this method
  /// </summary>
  public virtual void InteractHandler()
  {
  }

  /// <summary>
  /// Interact with object using some other object (e.g. insert cog into mechanism)
  /// </summary>
  /// <param name="obj">Object to use upon this one</param>
  public virtual void InteractWithObject(WorldObjectBase obj)
  {
  }
}
