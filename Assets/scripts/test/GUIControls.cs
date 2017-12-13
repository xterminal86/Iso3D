using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIControls : MonoBehaviour 
{
  public Texture2D CursorWait;
  public Texture2D CursorLook;

  public void OnMouseEnterLook()
  {
    UnityEngine.Cursor.SetCursor(CursorLook, Vector2.zero, CursorMode.Auto);
  }

  public void OnMouseExitLook()
  {
    UnityEngine.Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
  }

  public void OnMouseEnterWait()
  {
    UnityEngine.Cursor.SetCursor(CursorWait, Vector2.zero, CursorMode.Auto);
  }

  public void OnMouseExitWait()
  {
    UnityEngine.Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
  }
}
