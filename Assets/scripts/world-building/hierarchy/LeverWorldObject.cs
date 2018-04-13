using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverWorldObject : WorldObjectBase 
{
  public float AnimationSpeed = 1.0f;
  public Animation AnimationComponent;

  bool _isDown = false;

  void Awake()
  {
    AnimationComponent["lever-down"].speed = AnimationSpeed;
    AnimationComponent["lever-up"].speed = AnimationSpeed;
  }

  string _animationName = string.Empty;
  public override void Interact()
  {
    if (!_isDown)
    {
      _animationName = "lever-down";
      _isDown = true;
    }
    else
    {
      _animationName = "lever-up";
      _isDown = false;
    }

    UnityEngine.Cursor.SetCursor(PartyController.Instance.WaitCursor, Vector2.zero, CursorMode.Auto);

    PartyController.Instance.LockMovement = true;

    AnimationComponent.Play(_animationName);
    StartCoroutine(WaitForAnimationEndRoutine());
  }

  IEnumerator WaitForAnimationEndRoutine()
  {
    while (AnimationComponent.IsPlaying(_animationName))
    {
      yield return null;
    }

    PartyController.Instance.LockMovement = false;
    UnityEngine.Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

    base.Interact();

    yield return null;
  }
}
