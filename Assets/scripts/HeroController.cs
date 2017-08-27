using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour 
{
  public Transform Model;

  public Animator AnimatorController;

  enum WALK_DIR
  {
    SW = 0,
    SE,
    NE,
    NW
  }

  Vector3 _originalScale = Vector3.one;
  Vector3 _scale = Vector3.one;

  WALK_DIR _currentDir = WALK_DIR.NE;

  bool _isWalking = false;
 
  void Awake()
  {
    _originalScale = Model.localScale;
    _scale = _originalScale;
  }

  Dictionary<WALK_DIR, bool> _walkStatus = new Dictionary<WALK_DIR, bool>();
  void Update()
  {    
    _walkStatus[WALK_DIR.NW] = Input.GetKey(KeyCode.Q);
    _walkStatus[WALK_DIR.SW] = Input.GetKey(KeyCode.A);
    _walkStatus[WALK_DIR.NE] = Input.GetKey(KeyCode.E);
    _walkStatus[WALK_DIR.SE] = Input.GetKey(KeyCode.D);

    AnimatorController.SetBool("walk-ne", _walkStatus[WALK_DIR.NE]);
    AnimatorController.SetBool("walk-nw", _walkStatus[WALK_DIR.NW]);
    AnimatorController.SetBool("walk-se", _walkStatus[WALK_DIR.SE]);
    AnimatorController.SetBool("walk-sw", _walkStatus[WALK_DIR.SW]);

    _isWalking = _walkStatus[WALK_DIR.NW] || _walkStatus[WALK_DIR.NE] || _walkStatus[WALK_DIR.SW] || _walkStatus[WALK_DIR.SE];

    if (!_isWalking)
    {
      switch (_currentDir)
      {
        case WALK_DIR.NE:
          AnimatorController.SetBool("stand-ne", true);
          break;

        case WALK_DIR.NW:
          AnimatorController.SetBool("stand-nw", true);
          break;

        case WALK_DIR.SW:
          AnimatorController.SetBool("stand-sw", true);
          break;

        case WALK_DIR.SE:
          AnimatorController.SetBool("stand-se", true);
          break;
      }
    }
    else
    {
      AnimatorController.SetBool("stand-ne", false);
      AnimatorController.SetBool("stand-nw", false);
      AnimatorController.SetBool("stand-se", false);
      AnimatorController.SetBool("stand-sw", false);

      foreach (var item in _walkStatus)
      {
        if (item.Value)
        {
          _currentDir = item.Key;
          break;
        }
      }

      switch (_currentDir)
      {
        case WALK_DIR.NE:
        case WALK_DIR.SE:          
          _scale.x = -_originalScale.x;
          break;

        case WALK_DIR.NW:          
        case WALK_DIR.SW:
          _scale.x = _originalScale.x;
          break;
      }
    }

    Model.localScale = _scale;
  }
}
