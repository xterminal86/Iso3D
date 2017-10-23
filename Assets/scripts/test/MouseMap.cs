using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMap : MonoBehaviour 
{
  public GameObject MouseSelection;

  public Texture2D MouseMapTexture;

	void Start () 
  {
    /*
    for (int x = 0; x < MouseMapTexture.width; x++)
    {
      for (int y = 0; y < MouseMapTexture.height; y++)
      {
        Color pixel = MouseMapTexture.GetPixel(x, y);
        Debug.Log(string.Format("Color at [{0}:{1}] => {2}", x, y, pixel));
      }
    }
    */
	}
	
	void Update () 
  {
    var pos = Input.mousePosition;

    float x = (int)(pos.x / MouseMapTexture.width);
    float y = (int)(pos.y / MouseMapTexture.height);

    int mouseMapX = (int)(pos.x % MouseMapTexture.width);
    int mouseMapY = (int)(pos.y % MouseMapTexture.height);

    //Debug.Log(x + " " + y + " " + dx + " " + dy);

    var pixel = MouseMapTexture.GetPixel(mouseMapX, mouseMapY);

    if (pixel.r == 1.0f && pixel.g == 0.0f && pixel.b == 0.0f)
    {
      x -= 0.5f;
      y += 0.25f;
    }
    else if (pixel.r == 1.0f && pixel.g == 1.0f && pixel.b == 0.0f)
    {
      x -= 0.5f;
      y -= 0.25f;
    }
    else if (pixel.r == 1.0f && pixel.g == 0.0f && pixel.b == 1.0f)
    {
      x += 0.5f;
      y += 0.25f;
    }
    else if (pixel.r == 0.0f && pixel.g == 1.0f && pixel.b == 1.0f)
    {
      x += 0.5f;
      y -= 0.25f;
    }

    MouseSelection.transform.position = new Vector3(x, y);
	}
}
