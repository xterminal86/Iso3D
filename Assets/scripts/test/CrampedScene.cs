using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrampedScene : MonoBehaviour 
{
  int _size = 100;
  void Start () 
  {
    GameObject holder = new GameObject("Holder");

    for (int x = -_size; x < _size; x++)
    {
      for (int z = -_size; z < _size; z++)
      {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.position = new Vector3(x * 2, 0.0f, z * 2);
        go.transform.parent = holder.transform;
      }
    }
	}	
}
