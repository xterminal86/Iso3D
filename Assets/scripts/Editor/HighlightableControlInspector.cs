using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEditor;
using UnityEditor.Events;

[CustomEditor(typeof(HighlightableControl), true)]
public class HighlightableControlInspector : Editor 
{  
  Dictionary<EventTriggerType, UnityAction<BaseEventData>> _methods = new Dictionary<EventTriggerType, UnityAction<BaseEventData>>();    
  public override void OnInspectorGUI()
  {
    HighlightableControl hc = target as HighlightableControl;

    if (hc == null) return;

    var et = hc.GetComponent<EventTrigger>();

    if (GUILayout.Button("Assign"))
    {      
      _methods.Clear();
      et.triggers.Clear();

      _methods.Add(EventTriggerType.PointerEnter, new UnityAction<BaseEventData>(hc.OnMouseEnter));
      _methods.Add(EventTriggerType.PointerExit, new UnityAction<BaseEventData>(hc.OnMouseExit));
      _methods.Add(EventTriggerType.PointerDown, new UnityAction<BaseEventData>(hc.OnMouseDown));

      foreach (var item in _methods)
      {
        EventTrigger.Entry e = new EventTrigger.Entry();
        e.eventID = item.Key;
        e.callback = new EventTrigger.TriggerEvent();
        et.triggers.Add(e);

        UnityEventTools.AddPersistentListener(et.triggers[et.triggers.Count - 1].callback, item.Value);
      }
    }

    DrawDefaultInspector();
  }
}
