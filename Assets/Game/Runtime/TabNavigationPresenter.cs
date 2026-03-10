using System;
using Zenject;

public class TabNavigationPresenter : IInitializable, IDisposable
{
   private readonly TabNavigationView  _view;
   private readonly SignalBus          _signalBus;

   public TabNavigationPresenter(TabNavigationView view, SignalBus signalBus)
   {
      _view      = view;
      _signalBus = signalBus;
   }

   public void Initialize()
   {
      _signalBus.Subscribe<SwitchTabSignal>(OnSwitchTab);
   }

   private void OnSwitchTab(SwitchTabSignal signal)
   {
      _view.ShowTab(signal.Tab);

      _signalBus.Fire(new ShowTabSignal(signal.Tab));
   }

   public void Dispose()
   {
      _signalBus.Unsubscribe<SwitchTabSignal>(OnSwitchTab);
   }
}