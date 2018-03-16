using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTest : MonoBehaviour 
{
  public Texture2D LookCursor;

  void Start()
  {
    FormTalk.Instance.Initialize();
    LevelLoader.Instance.Initialize();
    CameraController.Instance.Initialize();
    PartyController.Instance.Initialize();

    PartyController.Instance.SetPlayerPosition(new Int3(0, 0, 0), 0.0f);

    PartyController.Instance.AddToParty("Delia");
  }

  RaycastHit _hitInfo;
  void Update()
  {
    Ray r = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
    if (Physics.Raycast(r.origin, r.direction, out _hitInfo, Mathf.Infinity))
    {
      var wob = _hitInfo.collider.GetComponent<WorldObjectBase>();
      if (wob != null && wob.InteractableObjectType != GlobalConstants.InteractableObjects.NONE)
      {
        UnityEngine.Cursor.SetCursor(LookCursor, Vector2.zero, CursorMode.Auto);

        if (Input.GetMouseButtonDown(1))
        {          
          FormTalk.Instance.Inspect();
        }
      }
      else
      {
        UnityEngine.Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
      }
    }      
  }
}
