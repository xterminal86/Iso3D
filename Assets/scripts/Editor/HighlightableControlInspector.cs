using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEditor;

[CustomEditor(typeof(HighlightableControl), true)]
public class HighlightableControlInspector : Editor 
{
  /*
  void Awake()
  {
    HighlightableControl hc = target as HighlightableControl;

    var et = hc.GetComponent<EventTrigger>();

    if (et.triggers.Count == 0)
    {      
      EventTrigger.Entry entry = new EventTrigger.Entry();
      entry.eventID = EventTriggerType.PointerDown;
      entry.callback = new EventTrigger.TriggerEvent();
      UnityAction<BaseEventData> call = new UnityAction<BaseEventData>(hc.OnMouseDown);
      entry.callback.AddListener(call);
      et.triggers.Add(entry);

      entry = new EventTrigger.Entry();
      entry.eventID = EventTriggerType.PointerEnter;
      entry.callback = new EventTrigger.TriggerEvent();
      call = new UnityAction<BaseEventData>(hc.OnMouseEnter);
      entry.callback.AddListener(call);
      et.triggers.Add(entry);

      entry = new EventTrigger.Entry();
      entry.eventID = EventTriggerType.PointerExit;
      entry.callback = new EventTrigger.TriggerEvent();
      call = new UnityAction<BaseEventData>(hc.OnMouseExit);
      entry.callback.AddListener(call);
      et.triggers.Add(entry);
    }
  }
  */

  public override void OnInspectorGUI()
  {
    HighlightableControl hc = target as HighlightableControl;

    if (hc == null) return;

    if (GUILayout.Button("Assign"))
    {
      hc.AssignEventMethods();
    }

    DrawDefaultInspector();
  }
}
