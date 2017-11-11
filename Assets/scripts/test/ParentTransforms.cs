using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentTransforms : MonoBehaviour 
{
  public GameObject Object1;
  public GameObject Object2;
  public GameObject Object3;
  public GameObject Object4;

  void Start()
  {
    Instantiate(Object2, new Vector3(2.0f, 0.0f, 0.0f), Quaternion.identity, Object1.transform);
  }
}
