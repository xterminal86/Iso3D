using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour 
{
  public Transform PlayerTail;

  bool _isApproaching = false;

  Vector3 _posToReach = Vector3.zero;
  void Update()
  {
    float d = Vector3.Distance(PlayerTail.position, transform.position);

    if (d > 0.1f)
    {
      Vector3 hero = new Vector3(transform.position.x, 0.0f, transform.position.z);

      Vector3 dir = PlayerTail.position - hero;
      dir.y = 0.0f;
      dir.Normalize();

      Vector3 v1 = new Vector3(0.0f, 0.0f, 1.0f);
      float angle = Vector3.Angle(v1, dir);
      float angle360 = Mathf.Sign(Vector3.Cross(v1, dir).y) < 0 ? (360 - angle) % 360 : angle;

      Quaternion fromRotation = transform.rotation;
      Quaternion toRotation = Quaternion.AngleAxis(angle360, Vector3.up);

      transform.rotation = Quaternion.Slerp(fromRotation, toRotation, Time.smoothDeltaTime * GlobalConstants.HeroRotateSpeed);

      Vector3 pos = transform.position;
      pos += dir * (2.0f * Time.smoothDeltaTime);
      transform.position = pos;
    }
  }
}
