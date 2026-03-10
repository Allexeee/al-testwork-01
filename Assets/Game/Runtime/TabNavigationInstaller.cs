using Zenject;

public class TabNavigationInstaller : MonoInstaller
{
   public TabConfigSO       tabConfig;
   public TabNavigationView navView;

   public override void InstallBindings()
   {
      SignalBusInstaller.Install(Container);
      Container.DeclareSignal<SwitchTabSignal>();
      Container.DeclareSignal<ShowTabSignal>();

      Container.BindInstance(tabConfig).AsSingle();
      Container.BindInstance(navView).AsSingle();

      Container.BindInterfacesAndSelfTo<TabNavigationPresenter>().AsSingle();
   }
}