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

    LevelToLoad.text = _zoneRef.ExitZoneToSave.LevelNameToLoad;
    PosX.text = _zoneRef.ExitZoneToSave.ArrivalMapPosition.X.ToString();
    PosY.text = _zoneRef.ExitZoneToSave.ArrivalMapPosition.Y.ToString();
    PosZ.text = _zoneRef.ExitZoneToSave.ArrivalMapPosition.Z.ToString();
    RotationAngle.text = _zoneRef.ExitZoneToSave.ArrivalCharacterAngle.ToString();
  }

  public void OnEndEditHandler()
  {
    Int3 arrivalPosition = Int3.Zero;

    float angle = string.IsNullOrEmpty(RotationAngle.text) ? 0.0f : float.Parse(RotationAngle.text);

    arrivalPosition.X = string.IsNullOrEmpty(PosX.text) ? 0 : int.Parse(PosX.text);
    arrivalPosition.Y = string.IsNullOrEmpty(PosY.text) ? 0 : int.Parse(PosY.text);
    arrivalPosition.Z = string.IsNullOrEmpty(PosZ.text) ? 0 : int.Parse(PosZ.text);

    _zoneRef.ExitZoneToSave.ArrivalCharacterAngle = angle;
    _zoneRef.ExitZoneToSave.ArrivalMapPosition.Set(arrivalPosition);
    _zoneRef.ExitZoneToSave.LevelNameToLoad = LevelToLoad.text;

    GameEditorScript.EnteringText = false;
  }

  public void SelectHandler()
  {
    GameEditorScript.EnteringText = true;
  }
}
