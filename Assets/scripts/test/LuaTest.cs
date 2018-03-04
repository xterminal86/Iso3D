using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

public class LuaTest : MonoBehaviour 
{
  void Awake()
  {
    TextAsset ta = Resources.Load("lua/test") as TextAsset;

    Script luaScript = new Script();

    DynValue res = luaScript.DoString(ta.text);

    var result = luaScript.Call(luaScript.Globals["fact"], 3);
    Debug.Log(result);

    result = luaScript.Call(luaScript.Globals["hello"]);
    Debug.Log(result);

    result = luaScript.Call(luaScript.Globals["get_list"]);
    Debug.Log(result);

    result = luaScript.Call(luaScript.Globals["get_dic"]);

    foreach (var key in result.Table.Keys)
    {
      Debug.Log(key);
      Debug.Log(result.Table.Get(key));
    }

    Dictionary<string, string> test = new Dictionary<string, string>() { { "foo", "42" } };

    result = luaScript.Call(luaScript.Globals["test_dic"], test);
    Debug.Log(result);
  }
}
