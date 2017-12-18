using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ActorLogicBase), true)]
public class ActorLogicInspector : Editor 
{
  public override void OnInspectorGUI()
  {
    ActorLogicBase al = target as ActorLogicBase;

    if (al == null) return;

    DrawDefaultInspector();

    al.ActorStatsObject.CharName = EditorGUILayout.TextField("Actor Name", al.ActorStatsObject.CharName);
    EditorGUILayout.HelpBox("Strength (STR)", MessageType.Info);
    al.ActorStatsObject.StrengthStat.X = EditorGUILayout.IntField("\tcurrent", al.ActorStatsObject.StrengthStat.X);
    al.ActorStatsObject.StrengthStat.Y = EditorGUILayout.IntField("\tmax", al.ActorStatsObject.StrengthStat.Y);
    EditorGUILayout.HelpBox("Defence (DEF)", MessageType.Info);
    al.ActorStatsObject.DefenceStat.X = EditorGUILayout.IntField("\tcurrent", al.ActorStatsObject.DefenceStat.X);
    al.ActorStatsObject.DefenceStat.Y = EditorGUILayout.IntField("\tmax", al.ActorStatsObject.DefenceStat.Y);
    EditorGUILayout.HelpBox("Speed (SPD)", MessageType.Info);
    al.ActorStatsObject.SpeedStat.X = EditorGUILayout.IntField("\tcurrent", al.ActorStatsObject.SpeedStat.X);
    al.ActorStatsObject.SpeedStat.Y = EditorGUILayout.IntField("\tmax", al.ActorStatsObject.SpeedStat.Y);
    EditorGUILayout.HelpBox("Hitpoints (HP)", MessageType.Info);
    al.ActorStatsObject.Hitpoints.X = EditorGUILayout.IntField("\tcurrent", al.ActorStatsObject.Hitpoints.X);
    al.ActorStatsObject.Hitpoints.Y = EditorGUILayout.IntField("\tmax", al.ActorStatsObject.Hitpoints.Y);
    EditorGUILayout.HelpBox("Affinity (AFY)", MessageType.Info);
    al.ActorStatsObject.UnityPoints.X = EditorGUILayout.IntField("\tcurrent", al.ActorStatsObject.UnityPoints.X);
    al.ActorStatsObject.UnityPoints.Y = EditorGUILayout.IntField("\tmax", al.ActorStatsObject.UnityPoints.Y);

    if (GUI.changed)
    {
      EditorUtility.SetDirty(al);
      AssetDatabase.SaveAssets();
    }
  }
}
