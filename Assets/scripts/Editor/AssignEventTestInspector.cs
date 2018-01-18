using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEditor.Events;

[CustomEditor(typeof(AssignEventTest))]
public class AssignEventTestInspector : Editor 
{
  public override void OnInspectorGUI()
  {
    AssignEventTest aet = target as AssignEventTest;

    if (aet == null) return;

    if (GUILayout.Button("Assign"))
    {
      var et = aet.GetComponent<EventTrigger>();

      var targetinfo = UnityEvent.GetValidMethodInfo(aet, "TestMethod", new Type[] { typeof(BaseEventData) });

      EventTrigger.Entry e = new EventTrigger.Entry();
      e.eventID = EventTriggerType.PointerDown;
      e.callback = new EventTrigger.TriggerEvent();
      et.triggers.Add(e);

      UnityEventTools.AddPersistentListener(et.triggers[0].callback, aet.TestMethod);
    }

    DrawDefaultInspector();
  }
}
