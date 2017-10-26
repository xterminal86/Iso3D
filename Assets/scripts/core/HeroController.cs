using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour 
{
  public Rigidbody RigidbodyComponent;

  public Transform Model;

  public Animator AnimatorController;

  enum WALK_DIR
  {
    UP = 0,
    NE,
    RIGHT,
    SE,
    DOWN,
    SW,
    LEFT,
    NW
  }

  WALK_DIR _oldWalkDir = WALK_DIR.DOWN;
  WALK_DIR _currentWalkDir = WALK_DIR.DOWN;

  bool _isWalking = false;
 
  void Awake()
  {    
    CameraController.Instance.LockOnHero(this);
  }

  RaycastHit _hitInfo;
  Dictionary<WALK_DIR, bool> _walkStatus = new Dictionary<WALK_DIR, bool>();
  Dictionary<WALK_DIR, string> _animatorWalkStatesByDir = new Dictionary<WALK_DIR, string>() 
  {
    { WALK_DIR.UP, "up" },
    { WALK_DIR.NE, "ne" },
    { WALK_DIR.RIGHT, "right" },
    { WALK_DIR.SE, "se" },
    { WALK_DIR.DOWN, "down" },
    { WALK_DIR.SW, "sw" },
    { WALK_DIR.LEFT, "left" },
    { WALK_DIR.NW, "nw" }
  };

  void Update()
  { 
    if (Input.GetMouseButton(0))
    {
      Ray r = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
      int mask = LayerMask.GetMask("MouseMap");
      if (Physics.Raycast(r.origin, r.direction, out _hitInfo, Mathf.Infinity, mask))
      {
        Vector3 s = new Vector3(_hitInfo.collider.transform.position.x, 0.0f, _hitInfo.collider.transform.position.z);
        Vector3 e = new Vector3(RigidbodyComponent.position.x, 0.0f, RigidbodyComponent.position.z);
        Vector3 dir = s - e;
        dir.Normalize();
        Vector3 v1 = new Vector3(1.0f, 0.0f, -1.0f);
        float angle = Vector3.Angle(v1, dir);
        float angle360 = Mathf.Sign(Vector3.Cross(v1, dir).y) < 0 ? (360 - angle) % 360 : angle;
        string debugText = string.Format("{0} {1} {2}\n", dir, angle, angle360);
        DebugForm.Instance.DebugText.text = debugText;
        _direction = dir;

        SetSpriteDirection(angle360);
      }
    }
    else
    {      
      SetStopDirection();
      _direction = Vector3.zero;
    }
  }

  Dictionary<WALK_DIR, string> _animatorStopStatesByDir = new Dictionary<WALK_DIR, string>()
  {
    { WALK_DIR.UP, "up-stand" },
    { WALK_DIR.NE, "ne-stand" },
    { WALK_DIR.RIGHT, "right-stand" },
    { WALK_DIR.SE, "se-stand" },
    { WALK_DIR.DOWN, "down-stand" },
    { WALK_DIR.SW, "sw-stand" },
    { WALK_DIR.LEFT, "left-stand" },
    { WALK_DIR.NW, "nw-stand" }
  };

  void SetStopDirection()
  {    
    AnimatorController.Play(_animatorStopStatesByDir[_currentWalkDir]);
  }

  void SetSpriteDirection(float angle360)
  {
    if ((angle360 > 0 && angle360 < 22.5f) || (angle360 > 337.5f && angle360 < 360.0f))
    {
      _currentWalkDir = WALK_DIR.RIGHT;
      _walkStatus[WALK_DIR.RIGHT] = true;
      DebugForm.Instance.DebugText.text += "Right";
    }
    else if (angle360 > 22.5f && angle360 < 67.5f)
    {
      _currentWalkDir = WALK_DIR.SE;
      _walkStatus[WALK_DIR.SE] = true;
      DebugForm.Instance.DebugText.text += "SouthEast";
    }
    else if (angle360 > 67.5f && angle360 < 112.5f)
    {
      _currentWalkDir = WALK_DIR.DOWN;
      _walkStatus[WALK_DIR.DOWN] = true;
      DebugForm.Instance.DebugText.text += "Down";
    }
    else if (angle360 > 112.5f && angle360 < 157.5f)
    {
      _currentWalkDir = WALK_DIR.SW;
      _walkStatus[WALK_DIR.SW] = true;
      DebugForm.Instance.DebugText.text += "SouthWest";
    }
    else if (angle360 > 157.5f && angle360 < 202.5f)
    {
      _currentWalkDir = WALK_DIR.LEFT;
      _walkStatus[WALK_DIR.LEFT] = true;
      DebugForm.Instance.DebugText.text += "Left";
    }
    else if (angle360 > 202.5f && angle360 < 247.5f)
    {
      _currentWalkDir = WALK_DIR.NW;
      _walkStatus[WALK_DIR.NW] = true;
      DebugForm.Instance.DebugText.text += "NorthWest";
    }
    else if (angle360 > 247.5f && angle360 < 292.5f)
    {
      _currentWalkDir = WALK_DIR.UP;
      _walkStatus[WALK_DIR.UP] = true;
      DebugForm.Instance.DebugText.text += "Up";
    }
    else if (angle360 > 292.5f && angle360 < 337.5f)
    {
      _currentWalkDir = WALK_DIR.NE;
      _walkStatus[WALK_DIR.NE] = true;
      DebugForm.Instance.DebugText.text += "NorthEast";
    }

    AnimatorController.Play(_animatorWalkStatesByDir[_currentWalkDir]);        

    if (_currentWalkDir != _oldWalkDir)
    {      
      _oldWalkDir = _currentWalkDir;
    }
  }

  Vector3 _direction = Vector3.zero;
  void FixedUpdate()
  {
    RigidbodyComponent.MovePosition(RigidbodyComponent.position + _direction * (GlobalConstants.HeroMoveSpeed * Time.fixedDeltaTime));
  }
}
