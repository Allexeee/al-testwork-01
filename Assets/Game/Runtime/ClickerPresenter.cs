using System;
using UnityEngine;
using Zenject;

public class ClickerPresenter : ITickable, IInitializable, IDisposable
{
   readonly ClickerModel    _model;
   readonly ClickerView     _view;
   readonly ClickerConfigSO _config;

   float _autoClickTimer;
   float _energyRegenTimer;

   public ClickerPresenter(ClickerModel model, ClickerView view, ClickerConfigSO config)
   {
      _model  = model;
      _view   = view;
      _config = config;
   }

   public void Initialize()
   {
      _view.OnButtonClicked += HandleClick;
      UpdateUI();
   }

   public void Tick()
   {
      _autoClickTimer += Time.deltaTime;
      if (_autoClickTimer >= _config.autoClickInterval)
      {
         _autoClickTimer = 0;
         if (_model.CanCollect())
         {
            _model.Collect();
            _view.PlayClickVFX(_model.CountCollectPerClick());
            _view.PlayClickAutoSFX();
            UpdateUI();
         }
      }

      _energyRegenTimer += Time.deltaTime;
      if (_energyRegenTimer >= _config.energyRegenInterval)
      {
         _energyRegenTimer = 0;
         _model.RegenEnergy(out var restored);
         if (restored > 0)
            _view.PlayPopupEnergyRestored(restored);

         UpdateUI();
      }
   }

   void HandleClick()
   {
      if (_model.CanCollect())
      {
         _model.Collect();
         _view.PlayClickVFX(_model.CountCollectPerClick());
         _view.PlayClickSFX();
         UpdateUI();
      }
   }

   private void UpdateUI()
   {
      _view.SetCurrency(_model.Currency);
      _view.SetEnergy(_model.Energy);
   }

   public void Dispose()
   {
      _view.OnButtonClicked -= HandleClick;
   }
}