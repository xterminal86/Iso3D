using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject
{
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

  /// <summary>
  /// Interact with object using hand (e.g. pull lever)
  /// </summary>
  public virtual void Interact()
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
