using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour 
{
  public Animator AnimatorController;

  enum WALK_DIR
  {
    SW = 0,
    SE,
    NE,
    NW
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
  }
}
