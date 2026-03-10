using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Clips")]
public class SFXAudioConfigSO : ScriptableObject
{
   public List<AudioClip> clips;
   public float           pitchMin  = 1f;
   public float           pitchMax  = 1f;
   public float           volumeMin = 1f;
   public float           volumeMax = 1f;
}