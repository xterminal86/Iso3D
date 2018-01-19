using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerRingMaker : MonoBehaviour 
{
  public GameObject PowerRing;

  public int RingsNumber = 4;
  public float SpawnInterval = 0.5f;

  Color _ringsColor = Color.white;
  public void Spawn(Vector3 pos, Color c)
  {
    _ringsColor = c;

    StartCoroutine(SpawnRingsRoutine(pos));
  }

  IEnumerator SpawnRingsRoutine(Vector3 pos)
  {
    float timer = 0.0f;
    int count = 0;

    for (int i = 0; i < RingsNumber; i++)
    {
      while (timer < SpawnInterval)
      {
        timer += Time.smoothDeltaTime;

        yield return null;
      }

      var ring = Instantiate(PowerRing, pos, Quaternion.identity);
      ring.GetComponentInChildren<Renderer>().material.SetColor("_TintColor", _ringsColor);

      timer = 0.0f;
    }

    yield return null;
  }
}
