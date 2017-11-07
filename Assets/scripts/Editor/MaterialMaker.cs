using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Creates materials for transition tiles for two given textures (grass to cobblestone, grass to sand etc.)
/// </summary>
public class MaterialMaker : EditorWindow 
{
  Texture2D _texture1;
  Texture2D _texture2;

  string _customTextureName1 = string.Empty;
  string _customTextureName2 = string.Empty;

  [MenuItem("Window/Create Transition Materials")]
  public static void ShowWindow() 
  {
    var w = EditorWindow.GetWindow(typeof(MaterialMaker));
    float width = 640;
    float height = 320;
    float x = Screen.width / 2 - width / 2;
    float y = Screen.height / 2 - height / 2;
    w.position = new Rect(x, y, width, height);
  }

  void OnGUI()
  {
    GUILayout.Label("Create all transition materials for given textures", EditorStyles.boldLabel);

    _texture1 = (Texture2D)EditorGUILayout.ObjectField("Texture 1", _texture1, typeof(Texture2D), false);
    _texture2 = (Texture2D)EditorGUILayout.ObjectField("Texture 2", _texture2, typeof(Texture2D), false);

    EditorGUILayout.HelpBox("Custom names for compound material name (first letters of corresponding textures will be used if left empty)", MessageType.Info);

    _customTextureName1 = string.Empty;
    _customTextureName2 = string.Empty;

    _customTextureName1 = EditorGUILayout.TextField("Name 1", _customTextureName1);
    _customTextureName2 = EditorGUILayout.TextField("Name 2", _customTextureName2);

    if (GUILayout.Button("Generate Transition Materials"))
    {
      if (_texture1 == null || _texture2 == null)
      {
        Debug.LogWarning("Please set textures before proceeding!");
        return;
      }

      if (_customTextureName1 == string.Empty)
      {
        _customTextureName1 = _texture1.name[0].ToString();
      }

      if (_customTextureName2 == string.Empty)
      {
        _customTextureName2 = _texture2.name[0].ToString();
      }

      string folderName = string.Format("blend-{0}{1}", _customTextureName1, _customTextureName2);
      string path = "Assets/materials";
      string fullName = path + "/" + folderName;
      if (Directory.Exists(fullName))
      {
        Debug.LogWarning(fullName + " - folder exists!");
        return;
      }

      AssetDatabase.CreateFolder(path, folderName);

      float progress = 0.0f;

      var res = Directory.GetFiles("Assets/textures/masks", "*.png");

      float progressDelta = 1.0f / res.Length;

      foreach (var item in res)
      {
        string nameFixed = item.Replace('\\', '/');

        Texture2D mask = (Texture2D)AssetDatabase.LoadAssetAtPath(nameFixed, typeof(Texture2D));

        string suffix = mask.name.Replace("mask", "");

        string materialName = string.Format("{0}{1}.mat", folderName, suffix);
        string assetPath = string.Format("{0}/{1}", fullName, materialName);

        Material m = new Material(Shader.Find("Custom/MaskedBlending"));
        m.SetTexture("_MainTex", _texture1);
        m.SetTexture("_SecTex", _texture2);
        m.SetTexture("_FilTex", mask);

        AssetDatabase.CreateAsset(m, assetPath);

        progress += progressDelta;

        EditorUtility.DisplayProgressBar("Processing...", assetPath, progress);
      }

      EditorUtility.ClearProgressBar();
    }
  }
}
