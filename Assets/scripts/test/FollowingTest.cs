using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingTest : MonoBehaviour 
{
  RaycastHit _hitInfo;

  Vector3 _direction = Vector3.zero;
  Quaternion _fromRotation = Quaternion.identity;
  Quaternion _toRotation = Quaternion.identity;

  void Update()
  {
    if (Input.GetMouseButton(0))
    {
      Ray r = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
      if (Physics.Raycast(r.origin, r.direction, out _hitInfo, Mathf.Infinity))
      {
        Plane hPlane = new Plane(Vector3.up, _hitInfo.collider.transform.position);
        float distance = 0; 
        Vector3 point = Vector3.zero;
        if (hPlane.Raycast(r, out distance))
        {
          point = r.GetPoint(distance);
          point.y = 0.0f;
        }

        Vector3 hero = new Vector3(transform.position.x, 0.0f, transform.position.z);

        Vector3 dir = point - hero;
        dir.Normalize();

        _direction = dir;

        Vector3 v1 = new Vector3(0.0f, 0.0f, 1.0f);
        float angle = Vector3.Angle(v1, dir);
        float angle360 = Mathf.Sign(Vector3.Cross(v1, dir).y) < 0 ? (360 - angle) % 360 : angle;

        _fromRotation = transform.rotation;
        _toRotation = Quaternion.AngleAxis(angle360, Vector3.up);

        transform.rotation = Quaternion.Slerp(_fromRotation, _toRotation, Time.smoothDeltaTime * GlobalConstants.HeroRotateSpeed);

        Vector3 pos = transform.position;
        pos += _direction * (2.0f * Time.smoothDeltaTime);
        transform.position = pos;
      }
    }
    else
    {
      _direction = Vector3.zero;
    }
  }
}
