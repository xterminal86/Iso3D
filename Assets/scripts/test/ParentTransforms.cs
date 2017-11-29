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
    // Specifying parent in the Instantiate call is equivalent to Transform.SetParent(..., true)
    // SetParent(..., true) means that instantiated object's position will be recalculated
    // with regard to parent so that its position will be at the same spot as position specified in
    // Instantiate call (which is in world space).
    // If SetParent call goes with false, then object's position specified in Instantiate call
    // will be treated as position relative to parent.
    // E.g. we instantiate object at (2, 0, 0), parenting it at object at (-2, 0, 0) in the Instantiate call directly.
    // This is equivalent to making SetParent(..., true). which will make position of instantiated object (4, 0, 0) 
    // - the same as world position (2, 0, 0).
    // If we to do SetParent(..., false), position will be (2, 0, 0) - world position (0, 0, 0).
    //
    // Example is below.

    Instantiate(Object2, new Vector3(2.0f, 0.0f, 0.0f), Quaternion.identity, Object1.transform);
    var go = Instantiate(Object2, new Vector3(2.0f, 0.0f, 0.0f), Quaternion.identity);
    go.transform.SetParent(Object1.transform, true);
    go = Instantiate(Object2, new Vector3(2.0f, 0.0f, 0.0f), Quaternion.identity);
    go.transform.SetParent(Object1.transform, false);
  }
}
