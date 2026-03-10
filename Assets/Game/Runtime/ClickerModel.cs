using UnityEngine;

public class ClickerModel
{
   public int Currency { get; private set; }
   public int Energy   { get; private set; }

   private ClickerConfigSO _config;

   public ClickerModel(ClickerConfigSO config)
   {
      _config  = config;
      Energy   = config.maxEnergy;
      Currency = 0;
   }

   public int CountCollectPerClick() => _config.currencyPerClick;
   
   public bool CanCollect() => Energy >= _config.clickEnergyCost;

   public void Collect()
   {
      Energy   -= _config.clickEnergyCost;
      Currency += _config.currencyPerClick;
   }

   public void RegenEnergy(out int restored)
   {
      var prev = Energy;
      Energy   = Mathf.Min(Energy + _config.energyRegenAmount, _config.maxEnergy);
      restored = Energy - prev;
   }
}

