using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController3D : MonoBehaviour 
{
  public Transform RaycastPoint;

  public Rigidbody RigidbodyComponent;
  public Animation AnimationComponent;

  Vector3 _direction = Vector3.zero;

  void Start()
  {
    AnimationComponent["running"].speed = 1.5f;
    AnimationComponent["walking"].speed = 1.0f;
    AnimationComponent["HeroStance"].speed = 2.0f;
    AnimationComponent["HeroDrawSword"].speed = 2.0f;
  }

  int _animationIndex = 0;
  List<string> _animations = new List<string>() 
  {
    "Bow1",
    "Bow2",
    "Bow3",
    "HeroDrawSword",
    "HeroStance",
    "HeroStanceIdle"
  };

  string _debugText = string.Empty;

  bool _battleState = false;

  float _heroMoveSpeed = 0.0f;

  bool _isRunning = false;

  RaycastHit _hitInfo;
  void Update()
  {     
    _debugText = "";

    if (Input.GetKeyDown(KeyCode.B))
    {
      _battleState = !_battleState;
    }

    _debugText += _battleState ? "WAR\n" : "PEACE\n";

    if (!_battleState)
    {
      ProcessWalk();
    }
    else
    {
      TestAnimations();
    }

    RaycastHit res;
    int mask = LayerMask.GetMask("Ramp");
    float raycastLength = 1.0f;
    Debug.DrawRay(RaycastPoint.position, Vector3.down * raycastLength, Color.green, 1.0f);
    if (Physics.Raycast(RaycastPoint.position, Vector3.down, out res, raycastLength, mask))
    {
      _heroMoveSpeed *= 0.5f;
      RigidbodyComponent.useGravity = false;
      Vector3 v = new Vector3(RigidbodyComponent.position.x, res.point.y, RigidbodyComponent.position.z);
      RigidbodyComponent.position = v;
    }
    else
    {      
      RigidbodyComponent.useGravity = true;
    }

    _debugText += string.Format("speed: {0}\n", _heroMoveSpeed);

    DebugForm.Instance.DebugText.text = _debugText;
  }

  void TestAnimations()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {      
      AnimationComponent.CrossFade(_animations[_animationIndex]);
      _animationIndex++;

      if (_animationIndex > _animations.Count - 1)
      {
        _animationIndex = 0;
      }
    }
  }

  void ProcessWalk()
  {
    if (Input.GetMouseButton(0))
    {
      _initializeIdleAnimationsOnce = true;

      Ray r = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
      int mask = LayerMask.GetMask("MouseMap");
      if (Physics.Raycast(r.origin, r.direction, out _hitInfo, Mathf.Infinity, mask))
      {
        Plane hPlane = new Plane(Vector3.up, transform.position);
        float distance = 0; 
        Vector3 point = Vector3.zero;
        if (hPlane.Raycast(r, out distance))
        {
          point = r.GetPoint(distance);
          point.y = 0.0f;
        }

        Vector3 hero = new Vector3(RigidbodyComponent.position.x, 0.0f, RigidbodyComponent.position.z);

        Vector3 dir = point - hero;
        dir.Normalize();

        Vector3 v1 = new Vector3(0.0f, 0.0f, 1.0f);
        float angle = Vector3.Angle(v1, dir);
        float angle360 = Mathf.Sign(Vector3.Cross(v1, dir).y) < 0 ? (360 - angle) % 360 : angle;
        _debugText += string.Format("[{0}] {1} {2:N1} {3:N1}\n", RigidbodyComponent.position, dir, angle, angle360);

        _direction = dir;

        _isRunning = Input.GetKey(KeyCode.LeftShift);
        _heroMoveSpeed = _isRunning ? GlobalConstants.HeroMoveSpeed * 2.0f : GlobalConstants.HeroMoveSpeed;

        if (_isRunning)
        {
          AnimationComponent.CrossFade("running", 0.1f);
        }
        else
        {
          AnimationComponent.CrossFade("walking", 0.1f);
        }

        RigidbodyComponent.rotation = Quaternion.AngleAxis(angle360, Vector3.up);
      }
    }
    else
    {
      _direction = Vector3.zero;
      PlayIdleAnimation();
    }
  }

  List<string> _idleAnimations = new List<string>() 
  {
    "Idle",
    "Idle2",
    "Idle3",
    "Idle4",
    "Idle5",
    "Idle6"
  };

  bool _initializeIdleAnimationsOnce = true;
  float _waitingTime = 0.0f;
  float _alarm = 0.0f;
  Vector2 _pauseRange = new Vector2(8.0f, 10.0f);
  int _lastPlayedIdleAnimationIndex = -1;
  void PlayIdleAnimation()
  {
    if (_initializeIdleAnimationsOnce)
    {
      _initializeIdleAnimationsOnce = false;
      _alarm = Random.Range(_pauseRange.x, _pauseRange.y);
      _waitingTime = 0.0f;
      AnimationComponent.CrossFade("Idle", 0.1f);
    }

    _waitingTime += Time.smoothDeltaTime;
    if (_waitingTime > _alarm)
    {
      _waitingTime = 0.0f;
      _alarm = Random.Range(_pauseRange.x, _pauseRange.y);
      int index = Random.Range(0, _idleAnimations.Count);
      if (index == _lastPlayedIdleAnimationIndex)
      {
        index++;

        if (index > _idleAnimations.Count - 1)
        {
          index = 1;
        }
      }
      _lastPlayedIdleAnimationIndex = index;
      AnimationComponent.CrossFade(_idleAnimations[index], 0.1f);
      if (_idleAnimations[index] != "Idle")
      {
        AnimationComponent.PlayQueued("Idle");
      }
    }
  }

  void FixedUpdate()
  {
    RigidbodyComponent.MovePosition(RigidbodyComponent.position + _direction * (_heroMoveSpeed * Time.fixedDeltaTime));
    CameraController.Instance.UpdateCameraPosition(RigidbodyComponent.position);
  }
}
