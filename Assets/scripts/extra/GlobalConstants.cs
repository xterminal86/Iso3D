using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public delegate void Callback();
public delegate void CallbackO(object sender);
public delegate void CallbackB(bool arg);

public class MyUnityEvent : UnityEvent<HighlightableControl>
{
}

public static class GlobalConstants 
{ 
  public const float ScaleFactor = 2.0f;
  public const float HeroMoveSpeed = 1.5f;
  public const float HeroRunSpeed = 4.0f;
  public const float HeroRotateSpeed = 10.0f;

  public const string FloorTemplatePrefabName = "1.floor-template";
  public const string PathMarkerPrefabName = "2.path-marker";
  public const string ExitZonePrefabName = "3.exit-zone";
  public const string WallTemplatePrefabName = "4.wall-template";


  /*
  public static int[] TransitionBitmasks = new int[]
  {
    100000000,
    001000000,
    000000001,
    000000100,
    101000000,
    001000001,
    000000101,
    100000100,
    100000001,
    001000100,
    101000001,
    001000101,
    100000101,
    101000101,
    111000000,
    001001001,
    000000111,
    100100100,
    101101101,
    111000111,
    111001001,
    001001111,
    100100111,
    111100100,
    111001111,
    101101111,
    111100111,
    111101101,
    111101111
  };
  */

  public static int[] TransitionBitmasks = new int[]
  {
    000000000, // - NONE
    010000000, // - U
    000001000, // - R
    000000010, // - D
    000100000, // - L
    000101000, // - RL
    010000010, // - UD
    010001000, // - UR
    000001010, // - RD
    000100010, // - DL
    010100000, // - LU
    010001010, // - URD
    000101010, // - RDL
    010100010, // - DLU
    010101000, // - LUR
    010101010, // - ALL
    100000000, // - C_UL
    001000000, // - C_UR
    000000001, // - C_DR
    000000100, // - C_DL
    101000000, // - C_ULUR
    001000001, // - C_URDR
    000000101, // - C_DRDL
    100000100, // - C_DLUL
    101000001, // - C_ULURDR
    001000101, // - C_URDRDL
    100000101, // - C_DRDLUL
    101000100, // - C_DLULUR
    100000001, // - C_ULDR
    001000100, // - C_URDL
    101000101 // - C_ALL
  };
    
  public static Dictionary<int, Transitions> TransitionTypeByMask = new Dictionary<int, Transitions>() 
  {
    { 000000000, Transitions.NONE },
    { 100000000, Transitions.C_UL },
    { 001000000, Transitions.C_UR },
    { 000000001, Transitions.C_DR },
    { 000000100, Transitions.C_DL },
    { 101000000, Transitions.C_ULUR },
    { 001000001, Transitions.C_URDR },
    { 000000101, Transitions.C_DRDL },
    { 100000100, Transitions.C_DLUL },
    { 101000001, Transitions.C_ULURDR },
    { 001000101, Transitions.C_URDRDL },
    { 100000101, Transitions.C_DRDLUL },
    { 101000100, Transitions.C_DLULUR },
    { 100000001, Transitions.C_ULDR },
    { 001000100, Transitions.C_URDL },
    { 101000101, Transitions.C_ALL },
    { 010000000, Transitions.U },
    { 000001000, Transitions.R },
    { 000000010, Transitions.D },
    { 000100000, Transitions.L },
    { 000101000, Transitions.RL },
    { 010000010, Transitions.UD },
    { 010001000, Transitions.UR },
    { 000001010, Transitions.RD },
    { 000100010, Transitions.DL },
    { 010100000, Transitions.LU },
    { 010001010, Transitions.URD },
    { 000101010, Transitions.RDL },
    { 010100010, Transitions.DLU },
    { 010101000, Transitions.LUR },
    { 010101010, Transitions.ALL },
  };

  public static Dictionary<Transitions, string> TransitionMaskTextureNameByType = new Dictionary<Transitions, string>()
  {
    { Transitions.U, "mask-u" },
    { Transitions.R, "mask-r" },
    { Transitions.D, "mask-d" },
    { Transitions.L, "mask-l" },
    { Transitions.UR, "mask-ur" },
    { Transitions.RD, "mask-rd" },
    { Transitions.DL, "mask-dl" },
    { Transitions.LU, "mask-lu" },
    { Transitions.UD, "mask-ud" },
    { Transitions.RL, "mask-rl" },
    { Transitions.URD, "mask-urd" },
    { Transitions.RDL, "mask-rdl" },
    { Transitions.DLU, "mask-dlu" },
    { Transitions.LUR, "mask-lur" },
    { Transitions.C_UL, "mask-c-ul" },
    { Transitions.C_UR, "mask-c-ur" },
    { Transitions.C_DR, "mask-c-dr" },
    { Transitions.C_DL, "mask-c-dl" },
    { Transitions.C_ULUR, "mask-c-ulur" },
    { Transitions.C_URDR, "mask-c-urdr" },
    { Transitions.C_DRDL, "mask-c-drdl" },
    { Transitions.C_DLUL, "mask-c-dlul" },
    { Transitions.C_ULURDR, "mask-c-ulurdr" },
    { Transitions.C_URDRDL, "mask-c-urdrdl" },
    { Transitions.C_DRDLUL, "mask-c-drdlul" },
    { Transitions.C_DLULUR, "mask-c-dlulur" },
    { Transitions.C_URDL, "mask-c-urdl" },
    { Transitions.C_ULDR, "mask-c-uldr" },
    { Transitions.C_ALL, "mask-c-all" },
    { Transitions.ALL, "mask-all" }
  };

  public enum Transitions
  {
    NONE = 0,
    U,
    R,
    D,
    L,
    UR,
    RD,
    DL,
    LU,
    UD,
    RL,
    URD,
    RDL,
    DLU,
    LUR,
    C_UL,
    C_UR,
    C_DR,
    C_DL,
    C_ULUR,
    C_URDR,
    C_DRDL,
    C_DLUL,
    C_ULURDR,
    C_URDRDL,
    C_DRDLUL,
    C_DLULUR,
    C_URDL,
    C_ULDR,
    C_ALL,
    ALL
  }
}



