using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController3D : MonoBehaviour 
{
  public Transform RaycastPoint;
  public Transform SwordHand;
  public Transform BackTransform;

  public GameObject Weapon;
  public GameObject Cloak;

  public Rigidbody RigidbodyComponent;
  public Animation AnimationComponent;

  Vector3 _direction = Vector3.zero;

  public Vector3 SwordHandPosition = new Vector3(-0.4f, 0.0f, -1.0f);
  public Vector3 SwordHandAngles = new Vector3(90.0f, 0.0f, 0.0f);
  public Vector3 SwordBackPosition = new Vector3(0.0f, 1.75f, -1.1f);
  public Vector3 SwordBackAngles = new Vector3(0.0f, 0.0f, -40.0f);

  void Start()
  {
    AnimationComponent["running"].speed = 1.5f;
    AnimationComponent["HeroStanceRun"].speed = 1.5f;
    AnimationComponent["walking"].speed = 1.0f;
    AnimationComponent["HeroStance"].speed = 2.0f;
    AnimationComponent["HeroDrawSword"].speed = 1.5f;
    AnimationComponent["HeroSheathSword"].speed = 1.5f;
    AnimationComponent["HeroAttack1"].speed = 1.5f;
    AnimationComponent["HeroBlock"].speed = 2.0f;
  }

  int _animationIndex = 0;
  /*
  List<string> _animations = new List<string>() 
  {
    "Bow1",
    "Bow2",
    "Bow3",
    "HeroDrawSword",
    "HeroStance",
    "HeroStanceIdle",
    "HeroStanceRun",
    "HeroAttack1"
  };
  */

  List<string> _animations = new List<string>() 
  {
    "HeroDrawSword",
    "HeroStance",
    "HeroStanceIdle",
    "HeroBlock",
    "HeroStanceRun",
    "HeroAttack1",
    "HeroSheathSword"
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

    _debugText += _battleState ? "Press 'Space' to play animations\n" : "Press 'B' to enter animation play mode\n";

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
    //Debug.DrawRay(RaycastPoint.position, Vector3.down * raycastLength, Color.green, 1.0f);
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

    CameraController.Instance.UpdateCameraPosition(RigidbodyComponent.position);
  }

  void TestAnimations()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {  
      if (_animations[_animationIndex] == "HeroDrawSword")
      {
        StartCoroutine(DrawSwordRoutine());
      }
      else if (_animations[_animationIndex] == "HeroSheathSword")
      {
        StartCoroutine(SheathSwordRoutine());
      }

      AnimationComponent.CrossFade(_animations[_animationIndex], 0.1f);
      _animationIndex++;

      if (_animationIndex > _animations.Count - 1)
      {
        _animationIndex = 0;
      }
    }
  }

  IEnumerator SheathSwordRoutine()
  {
    while (AnimationComponent["HeroSheathSword"].normalizedTime < 0.5f)
    {
      yield return null;
    }

    Weapon.SetActive(false);

    Weapon.transform.parent = BackTransform;
    Weapon.transform.localPosition = SwordBackPosition;
    Weapon.transform.localEulerAngles = SwordBackAngles;

    yield return null;
  }

  IEnumerator DrawSwordRoutine()
  {
    while (AnimationComponent["HeroDrawSword"].normalizedTime < 0.5f)
    {
      yield return null;
    }

    Weapon.SetActive(true);

    Weapon.transform.parent = SwordHand;
    Weapon.transform.localPosition = SwordHandPosition;
    Weapon.transform.localEulerAngles = SwordHandAngles;

    yield return null;
  }

  Quaternion _fromRotation = Quaternion.identity;
  Quaternion _toRotation = Quaternion.identity;
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
        _heroMoveSpeed = _isRunning ? GlobalConstants.HeroRunSpeed : GlobalConstants.HeroMoveSpeed;

        if (_isRunning)
        {
          AnimationComponent.CrossFade("running", 0.1f);
        }
        else
        {
          AnimationComponent.CrossFade("walking", 0.1f);
        }
          
        _fromRotation = RigidbodyComponent.rotation;
        _toRotation = Quaternion.AngleAxis(angle360, Vector3.up);

        //RigidbodyComponent.rotation = Quaternion.AngleAxis(angle360, Vector3.up);
      }
    }
    else
    {
      _direction = Vector3.zero;
      PlayIdleAnimation();
    }
  }

  public void SetPlayerPosition(Int3 pos)
  {
    RigidbodyComponent.position = Util.MapToWorldCoordinates(new Vector3(pos.X, pos.Y, pos.Z));

    // When we modify position, cloth interprets it as a rapid movement, so it makes cloak
    // go haywire. Thus, we first move the player and then activate cloak.
    if (Cloak != null)
    {
      Cloak.SetActive(true);
    }
  }

  List<string> _idleAnimations = new List<string>() 
  {
    "Idle",
    "Idle2",
    "Idle3",
    "Idle4",
    "Idle5",
    "Idle6",
    "Idle7"
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
      AnimationComponent.CrossFade("Idle");
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
      AnimationComponent.CrossFade(_idleAnimations[index]);
      if (_idleAnimations[index] != "Idle")
      {
        AnimationComponent.PlayQueued("Idle");
      }
    }
  }

  void FixedUpdate()
  {
    RigidbodyComponent.MovePosition(RigidbodyComponent.position + _direction * (_heroMoveSpeed * Time.fixedDeltaTime));
    RigidbodyComponent.rotation = Quaternion.Slerp(_fromRotation, _toRotation, Time.fixedDeltaTime * GlobalConstants.HeroRotateSpeed);
  }
}
