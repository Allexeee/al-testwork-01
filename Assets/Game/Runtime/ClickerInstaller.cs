using UnityEngine;
using Zenject;

public class ClickerInstaller : MonoInstaller
{
   public Transform       poolRootCanvas;
   public Transform       poolRootWorld;
   public ClickerView     clickerView;
   public ClickerConfigSO clickerConfig;
   public ClickVFX        clickVFXPrefab;
   public FloatingText    floatingTextPrefab;

   public override void InstallBindings()
   {
      Container.BindInstance(clickerConfig).AsSingle();
      Container.Bind<ClickerModel>().AsSingle().WithArguments(clickerConfig);
      Container.BindInstance(clickerView);
      Container.BindInterfacesAndSelfTo<ClickerPresenter>().AsSingle();
      Container.BindMemoryPool<ClickVFX, ClickVFXPool>()
               .WithInitialSize(3)
               .FromComponentInNewPrefab(clickVFXPrefab)
               .UnderTransform(poolRootWorld);

      Container.BindMemoryPool<FloatingText, FloatingTextPool>()
               .WithInitialSize(10)
               .FromComponentInNewPrefab(floatingTextPrefab)
               .UnderTransform(poolRootCanvas);
   }
}