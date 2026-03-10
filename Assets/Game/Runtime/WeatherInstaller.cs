using UnityEngine;
using Zenject;

public class WeatherInstaller : MonoInstaller
{
   public WeatherView     view;
   public WeatherItemView prefabItemView;
   public Transform       root;

   public override void InstallBindings()
   {
      Container.BindInstance(view).AsSingle();
      Container.Bind<WeatherService>().AsSingle();
      Container.BindInterfacesTo<WeatherPresenter>().AsSingle();
      
      Container.BindMemoryPool<WeatherItemView, WheatherItemViewPool>()
               .FromComponentInNewPrefab(prefabItemView)
               .UnderTransform(root);

   }
}