using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ClickerView : MonoBehaviour
{
   public Button   clickButton;
   public TMP_Text currencyText;
   public TMP_Text energyText;

   [Inject] ClickVFXPool     _vfxPool;
   [Inject] FloatingTextPool _floatingTextPool;
   [Inject] SignalBus        _signalBus;

   public event Action OnButtonClicked;

   public void SetCurrency(int amount)
   {
      currencyText.text = amount.ToString();
   }

   public void SetEnergy(int amount)
   {
      energyText.text = amount.ToString();
   }

   private void Awake()
   {
      clickButton.onClick.AddListener(() => OnButtonClicked?.Invoke());
   }

   public void PlayClickSFX()
   {
      _signalBus.Fire(new PlaySfxSignal(SFXEnum.ClickerClick, clickButton.transform.position));
   }

   public void PlayClickAutoSFX()
   {
      _signalBus.Fire(new PlaySfxSignal(SFXEnum.ClickerClickAuto, clickButton.transform.position));
   }

   public void PlayClickVFX(int collected)
   {
      var vfx = _vfxPool.Spawn();
      vfx.Play(clickButton.transform.position, () => _vfxPool.Despawn(vfx));
      _floatingTextPool.Spawn($"+ {collected}$", clickButton.transform.position);
   }

   public void PlayPopupEnergyRestored(int value)
   {
      var position = clickButton.transform.position;
      position.y += 1f;
      _floatingTextPool.Spawn($"+ {value} Energy", position);
   }
}