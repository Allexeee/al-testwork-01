using UnityEngine;

[CreateAssetMenu(fileName = "ClickerConfig", menuName = "Configs/ClickerConfig")]
public class ClickerConfigSO : ScriptableObject
{
   public int   currencyPerClick    = 1;
   public float autoClickInterval   = 3f; 
   public float energyRegenInterval = 10f;
   public int   energyRegenAmount   = 10;
   public int   maxEnergy           = 1000;
   public int   clickEnergyCost     = 1;
}