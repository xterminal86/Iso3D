using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallWorldObject : WorldObjectBase 
{
  public MeshRenderer WallQuad;

  Material _material;

  Texture2D _texture;

  [HideInInspector]
  public string TextureName = string.Empty;

  public void Init(string textureName)
  {
    TextureName = textureName;
    string tex1path = string.Format("textures/{0}", textureName);
    _texture = Resources.Load<Texture2D>(tex1path);

    Material m = Resources.Load<Material>("materials/floor-simple");
    _material = new Material(m);
    _material.SetTexture("_MainTex", _texture);

    WallQuad.material = _material;
  }
}
