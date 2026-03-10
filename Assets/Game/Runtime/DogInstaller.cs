using UnityEngine;
using Zenject;

public class DogInstaller : MonoInstaller
{
   public Transform   root;
   public DogItemView prefabItemView;
   public DogTabView  dogTabView;
   public PopupView   popupView;

   public override void InstallBindings()
   {
      Container.BindInterfacesAndSelfTo<DogTabPresenter>().AsSingle();
      Container.Bind<DogApiService>().AsSingle();
      Container.BindInstance(dogTabView).AsSingle();
      Container.BindInstance(popupView).AsSingle();

      Container.BindMemoryPool<DogItemView, DogItemViewPool>()
               .FromComponentInNewPrefab(prefabItemView)
               .UnderTransform(root);
   }
}