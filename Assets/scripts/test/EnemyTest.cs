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

    float t = 0.0f;
    float rotationTime = 0.5f;
    float dt = t / rotationTime;

    Quaternion fromR = RigidbodyComponent.rotation;
    Quaternion to = Quaternion.LookRotation(dir, Vector3.up);

    while ((int)dt != 1)
    {
      t += Time.smoothDeltaTime;
      dt = t / rotationTime;
      dt = Mathf.Clamp(dt, 0.0f, 1.0f);

      Quaternion q = Quaternion.Lerp(fromR, to, dt);

      RigidbodyComponent.rotation = q;

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

  float _movingTime = 1.0f;

  Vector3 _initialPosition = Vector3.zero;
  IEnumerator ApproachTargetRoutine()
  {
    float d = Vector3.Distance(RigidbodyComponent.position, Target.position);
    Vector3 from = new Vector3(RigidbodyComponent.position.x, RigidbodyComponent.position.y, RigidbodyComponent.position.z);

    AnimationComponent.CrossFade("axe-run");

    float t2 = 0.0f;
    float dt = t2 / _movingTime;

    while (d > 1.0f)
    {
      d = Vector3.Distance(RigidbodyComponent.position, Target.position);

      t2 += Time.smoothDeltaTime;
      dt = t2 / _movingTime;
      dt = Mathf.Clamp(dt, 0.0f, 1.0f);

      Vector3 pos = Vector3.Lerp(from, Target.position, dt);
      RigidbodyComponent.position = pos;

      yield return null;
    }

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

    float t = 0.0f;
    float rotationTime = 0.5f;
    float dt = t / rotationTime;

    Vector3 r = RigidbodyComponent.rotation.eulerAngles;

    Quaternion fromR = Quaternion.Euler(r.x, r.y, r.z);
    Quaternion to = Quaternion.Euler(r.x, r.y + 180.0f, r.z);

    while ((int)dt != 1)
    {
      t += Time.smoothDeltaTime;
      dt = t / rotationTime;
      dt = Mathf.Clamp(dt, 0.0f, 1.0f);

      Quaternion q = Quaternion.Lerp(fromR, to, dt);

      RigidbodyComponent.rotation = q;

      yield return null;
    }

    yield return null;
  }

  IEnumerator GoBackRoutine()
  {
    Vector3 fromPosition = new Vector3(RigidbodyComponent.position.x, RigidbodyComponent.position.y, RigidbodyComponent.position.z);
    Vector3 target = new Vector3(_initialPosition.x, 0.0f, _initialPosition.z);

    AnimationComponent.CrossFade("axe-run");

    float t = 0.0f;
    float dt = t / _movingTime;

    while ((int)dt != 1)
    {
      t += Time.smoothDeltaTime;
      dt = t / _movingTime;
      dt = Mathf.Clamp(dt, 0.0f, 1.0f);

      Vector3 pos = Vector3.Lerp(fromPosition, target, dt);
      RigidbodyComponent.position = pos;

      yield return null;
    }

    yield return null;
  }
}
