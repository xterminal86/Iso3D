using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTest : MonoBehaviour 
{  
  public DoorWorldObject Door;
  public LeverWorldObject Lever;

  void Start()
  {
    FormTalk.Instance.Initialize();
    LevelLoader.Instance.Initialize();
    CameraController.Instance.Initialize();
    PartyController.Instance.Initialize();

    PartyController.Instance.SetPlayerPosition(new Int3(0, 0, 0), 0.0f);

    PartyController.Instance.AddToParty("Delia");

    Callback doorCb = new Callback(Door.InteractHandler);

    Lever.Interactions.Add(doorCb);
  }

  RaycastHit _hitInfo;
  void Update()
  {
    // FIXME: cursor hack
    if (PartyController.Instance.LockMovement)    
    {
      return;
    }

    Ray r = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
    int mask = LayerMask.GetMask("Default");
    if (Physics.Raycast(r.origin, r.direction, out _hitInfo, Mathf.Infinity, mask))
    {
      var wob = _hitInfo.collider.GetComponent<WorldObjectBase>();
      if (wob != null && wob.InteractableObjectType != GlobalConstants.InteractableObjects.NONE)
      {
        UnityEngine.Cursor.SetCursor(PartyController.Instance.LookCursor, Vector2.zero, CursorMode.Auto);

        if (Input.GetMouseButtonDown(1))
        {          
          UnityEngine.Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

          FormTalk.Instance.Inspect();
        }
      }
      else if (wob != null && wob is LeverWorldObject)
      {
        UnityEngine.Cursor.SetCursor(PartyController.Instance.InteractCursor, Vector2.zero, CursorMode.Auto);

        if (Input.GetMouseButtonDown(1))
        {          
          (wob as LeverWorldObject).Interact();
        }
      }
      else
      {
        UnityEngine.Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
      }
    }      
  }
}
