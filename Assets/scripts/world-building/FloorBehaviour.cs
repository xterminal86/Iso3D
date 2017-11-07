using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBehaviour : MonoBehaviour 
{
  public MeshRenderer FloorPlane;

  Texture2D _texture1;
  Texture2D _texture2;
  Texture2D _maskTexture;

  Material _material;

  MapBlock _blockRef;
  public void Init(MapBlock block)
  {
    block.FloorBehaviourRef = this;
    _blockRef = block;

    string texture1Name = GlobalConstants.TextureNameByType[block.Texture1];

    string tex1path = string.Format("textures/{0}", texture1Name);
    _texture1 = Resources.Load<Texture2D>(tex1path);

    if (block.TransitionMask == 0)
    {      
      Material m = Resources.Load<Material>("materials/floor-simple");
      _material = new Material(m);
      _material.SetTexture("_MainTex", _texture1);
    }
    else
    {
      string texture2Name = GlobalConstants.TextureNameByType[block.Texture2];
      string tex2path = string.Format("textures/{0}", texture2Name);

      string maskTexPath = string.Format("masks/{0}", GlobalConstants.TransitionMaskTextureNameByType[block.Transition]);

      _texture2 = Resources.Load<Texture2D>(tex2path);
      _maskTexture = Resources.Load<Texture2D>(maskTexPath);

      Material m = Resources.Load<Material>("materials/floor-transition");

      _material = new Material(m);

      _material.SetTexture("_MainTex", _texture1);
      _material.SetTexture("_SecTex", _texture2);
      _material.SetTexture("_FilTex", _maskTexture);
    }

    FloorPlane.material = _material;
  }
}
