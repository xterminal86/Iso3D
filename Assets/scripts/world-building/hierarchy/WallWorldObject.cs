using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallWorldObject : WorldObjectBase 
{
  public MeshRenderer WallMesh;

  Material _material;

  Texture2D _texture;

  [HideInInspector]
  public string TextureName = string.Empty;

  void Awake()
  {
    SerializedObject = new SerializedWall();

    PrefabName = GlobalConstants.WallTemplatePrefabName;
  }

  public override void Init(SerializedWorldObject serializedObject)
  {
    SerializedObject = serializedObject;

    ApplyTexture((serializedObject as SerializedWall).TextureName);
  }

  public void ApplyTexture(string textureName)
  {    
    TextureName = textureName;
    string tex1path = string.Format("textures/{0}", textureName);
    _texture = Resources.Load<Texture2D>(tex1path);

    Material m = Resources.Load<Material>("materials/floor-simple");
    _material = new Material(m);
    _material.SetTexture("_MainTex", _texture);

    WallMesh.material = _material;
  }
}
