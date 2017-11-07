using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIControls : MonoBehaviour 
{
  public Texture2D CursorWait;

  public void OnMouseEnter()
  {
    UnityEngine.Cursor.SetCursor(CursorWait, Vector2.zero, CursorMode.Auto);
  }

  public void OnMouseExit()
  {
    UnityEngine.Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
  }
}
