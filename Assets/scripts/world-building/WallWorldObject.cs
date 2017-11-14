﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallWorldObject : WorldObjectBase 
{
  public MeshRenderer WallQuad1;
  public MeshRenderer WallQuad2;

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

    WallQuad1.material = _material;
    WallQuad2.material = _material;
  }
}
