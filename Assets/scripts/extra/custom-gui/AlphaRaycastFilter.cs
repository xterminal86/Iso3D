using UnityEngine;
using UnityEngine.UI;

// https://coeurdecode.com/game%20development/2015/10/18/non-rectangular-ui-buttons-in-unity/

public class AlphaRaycastFilter : MonoBehaviour, ICanvasRaycastFilter
{
  public float minAlpha;

  public RectTransform RectTransformRef;
  public Image ButtonImage;

  #region ICanvasRaycastFilter implementation

  public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
  {
    // Get normalized hit point within rectangle (aka UV coordinates originating from bottom-left)
    Vector2 rectPoint;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransformRef, screenPoint, eventCamera, out rectPoint);
    Vector2 normPoint = (rectPoint - RectTransformRef.rect.min);
    normPoint.x /= RectTransformRef.rect.width;
    normPoint.y /= RectTransformRef.rect.height;

    // Read pixel color at normalized hit point
    Texture2D texture = ButtonImage.sprite.texture;
    Color color = texture.GetPixel((int)(normPoint.x * texture.width), (int)(normPoint.y * texture.height));

    // Keep hits on pixels above minimum alpha
    return color.a > minAlpha;
  }

  #endregion
}