using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBlock
{
  public Vector3 ArrayCoordinates = Vector3.zero;
  public Vector3 WorldCoordinates = Vector3.zero;

  public GlobalConstants.TEXTURE_TYPE Texture1 = GlobalConstants.TEXTURE_TYPE.NONE;
  public GlobalConstants.TEXTURE_TYPE Texture2 = GlobalConstants.TEXTURE_TYPE.NONE;

  public bool SkipTransitionCheckHere = false;

  public FloorBehaviour FloorBehaviourRef;

  int _transitionMask = 0;
  public int TransitionMask
  {
    get { return _transitionMask; }
    set
    {
      _transitionMask = value;
      _transition = GlobalConstants.TransitionTypeByMask[_transitionMask];
    }
  }

  GlobalConstants.Transitions _transition = GlobalConstants.Transitions.NONE;
  public GlobalConstants.Transitions Transition
  {
    get { return _transition; }
  }

  public MapBlock()
  {
  }
}
