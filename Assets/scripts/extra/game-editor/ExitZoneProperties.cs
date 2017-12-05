using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitZoneProperties : BaseObjectProperties 
{
  public InputField LevelToLoad;
  public InputField PosX;
  public InputField PosY;
  public InputField PosZ;
  public InputField RotationAngle;

  public GameEditor GameEditorScript;

  ExitZoneObject _zoneRef;
  public override void Init(WorldObjectBase gameObject)
  { 
    _zoneRef = gameObject as ExitZoneObject;

    SerializedExitZone ez = _zoneRef.SerializedObject as SerializedExitZone;

    LevelToLoad.text = ez.LevelNameToLoad;
    PosX.text = ez.ArrivalMapPosition.X.ToString();
    PosY.text = ez.ArrivalMapPosition.Y.ToString();
    PosZ.text = ez.ArrivalMapPosition.Z.ToString();
    RotationAngle.text = ez.ArrivalCharacterAngle.ToString();
  }

  public void OnEndEditHandler()
  {
    Int3 arrivalPosition = Int3.Zero;

    float angle = string.IsNullOrEmpty(RotationAngle.text) ? 0.0f : float.Parse(RotationAngle.text);

    arrivalPosition.X = string.IsNullOrEmpty(PosX.text) ? 0 : int.Parse(PosX.text);
    arrivalPosition.Y = string.IsNullOrEmpty(PosY.text) ? 0 : int.Parse(PosY.text);
    arrivalPosition.Z = string.IsNullOrEmpty(PosZ.text) ? 0 : int.Parse(PosZ.text);

    SerializedExitZone ez = _zoneRef.SerializedObject as SerializedExitZone;

    ez.ArrivalCharacterAngle = angle;
    ez.ArrivalMapPosition.Set(arrivalPosition);
    ez.LevelNameToLoad = LevelToLoad.text;

    GameEditorScript.EnteringText = false;
  }

  public void SelectHandler()
  {
    GameEditorScript.EnteringText = true;
  }
}
