using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTest : MonoBehaviour 
{
  public PowerRingMaker PowerRingMakerScript;

  public Text InfoText;

  public Rigidbody RigidbodyComponent;

  public Transform Target;
  public Transform Direction;
  public Transform Direction90;

  public Animation AnimationComponent;

  bool _ready = false;
  void Start()
  {
    InfoText.text = "Preparing...";

    AnimationComponent["axe-run"].speed = 3.0f;
    AnimationComponent["axe-attack"].speed = 2.0f;
    AnimationComponent["axe-idle"].speed = 1.0f;

    _initialPosition = new Vector3(RigidbodyComponent.position.x, 0.0f, RigidbodyComponent.position.z);

    StartCoroutine(PrepareRoutine());
  }

  float _rotationSpeed = 400.0f;
  IEnumerator PrepareRoutine()
  {
    AnimationComponent.CrossFade("axe-idle");

    Vector3 target = new Vector3(Target.position.x, 0.0f, Target.position.z);

    Vector3 dir = target - RigidbodyComponent.position;
    dir.Normalize();

    Vector3 v1 = Direction.position - RigidbodyComponent.position;
    float angle = Vector3.Angle(v1, dir);
    float angle360 = Mathf.Sign(Vector3.Cross(v1, dir).y) < 0 ? (360 - angle) % 360 : angle;

    float initialAngle = RigidbodyComponent.rotation.eulerAngles.y;

    float counter = initialAngle;
    float cond = (initialAngle + angle360) % 360.0f;

    float sign = (counter < cond) ? 1.0f : -1.0f;

    bool condition = (sign > 0.0f) ? (counter < cond) : (counter > cond);

    Vector3 ea = RigidbodyComponent.rotation.eulerAngles;

    while ((sign > 0.0f) ? (ea.y < cond) : (ea.y > cond))    
    {      
      counter = sign * (Time.smoothDeltaTime * _rotationSpeed);

      ea = RigidbodyComponent.rotation.eulerAngles;
      ea.y += counter;
      ea.y = Mathf.Clamp(ea.y, (sign > 0.0f) ? initialAngle : cond, (sign == 1) ? cond : initialAngle);

      RigidbodyComponent.rotation = Quaternion.Euler(ea);

      yield return null;
    }

    _ready = true;

    InfoText.text = "Ready";

    yield return null;
  }

  bool _isProcessing = false;
  void Update()
  {
    if (_isProcessing || !_ready) return;

    if (Input.GetKeyDown(KeyCode.Space))
    {
      InfoText.text = "Attacking...";

      _isProcessing = true;
      StartCoroutine(AttackRoutine());
    }
  }

  IEnumerator AttackRoutine()
  {
    yield return StartCoroutine(SpawnRingsRoutine());
    yield return StartCoroutine(ApproachTargetRoutine());
    yield return StartCoroutine(AttackTargetRoutine());
    yield return StartCoroutine(TurnAroundRoutine());
    yield return StartCoroutine(GoBackRoutine());
    yield return StartCoroutine(PrepareRoutine());

    _isProcessing = false;

    yield return null;
  }

  IEnumerator SpawnRingsRoutine()
  {
    Color ringsColor;

    bool res = ColorUtility.TryParseHtmlString("#6AD65F", out ringsColor);

    PowerRingMakerScript.Spawn(RigidbodyComponent.position, ringsColor);

    float cond = PowerRingMakerScript.RingsNumber * PowerRingMakerScript.SpawnInterval + 0.5f;

    float timer = 0.0f;
    while (timer < cond)
    {
      timer += Time.smoothDeltaTime;

      yield return null;
    }

    yield return null;
  }

  Vector3 _initialPosition = Vector3.zero;
  IEnumerator ApproachTargetRoutine()
  {
    float d = Vector3.Distance(RigidbodyComponent.position, Target.position);

    Vector3 target = new Vector3(Target.position.x, 0.0f, Target.position.z);

    Vector3 dir = target - RigidbodyComponent.position;
    dir.Normalize();

    AnimationComponent.CrossFade("axe-run");

    while (d > 1.0f)
    {      
      _rbDir = dir;

      d = Vector3.Distance(RigidbodyComponent.position, Target.position);

      yield return null;
    }

    _rbDir = Vector3.zero;

    yield return null;
  }

  IEnumerator AttackTargetRoutine()
  {
    AnimationComponent.Play("axe-attack");
   
    while (AnimationComponent["axe-attack"].time < AnimationComponent["axe-attack"].length)
    {      
      yield return null;
    }

    yield return null;
  }

  IEnumerator TurnAroundRoutine()
  {    
    AnimationComponent.CrossFade("axe-idle");

    float currentAngle = RigidbodyComponent.rotation.eulerAngles.y;
    float cond = currentAngle + 180.0f;

    float origAngle = currentAngle;

    while (currentAngle < cond)    
    {
      currentAngle += Time.smoothDeltaTime * _rotationSpeed;

      currentAngle = Mathf.Clamp(currentAngle, origAngle, cond);

      Vector3 ea = RigidbodyComponent.rotation.eulerAngles;
      ea.y = currentAngle;
      RigidbodyComponent.rotation = Quaternion.Euler(ea);

      yield return null;
    }

    yield return null;
  }

  IEnumerator GoBackRoutine()
  {
    float d = Vector3.Distance(transform.position, _initialPosition);
    Vector3 target = new Vector3(_initialPosition.x, 0.0f, _initialPosition.z);

    Vector3 dir = target - transform.position;
    dir.Normalize();

    AnimationComponent.CrossFade("axe-run");

    while (d > 0.1f)
    {
      _rbDir = dir;

      d = Vector3.Distance(RigidbodyComponent.position, _initialPosition);

      yield return null;
    }

    _rbDir = Vector3.zero;

    yield return null;
  }

  Vector3 _rbDir = Vector3.zero;
  void FixedUpdate()
  {
    RigidbodyComponent.MovePosition(RigidbodyComponent.position + _rbDir * (Time.fixedDeltaTime * 6.0f));
  }
}
