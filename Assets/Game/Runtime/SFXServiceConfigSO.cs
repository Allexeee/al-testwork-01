using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/SFX Service")]
public class SFXServiceConfigSO : ScriptableObject
{
   public List<Entry> data;

   [Serializable]
   public class Entry
   {
      public SFXEnum       kind;
      public SFXAudioConfigSO elements;
   }
}

public enum SFXEnum
{
   None,
   Click,
   ClickerClick,
   ClickerClickAuto,
}
