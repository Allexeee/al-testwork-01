using Zenject;

public class ServicesInstaller : MonoInstaller
{
   public SFXServiceConfigSO sfxServiceConfigSo;
   public SFXAudio           prefabSfxAudio;
   
   public override void InstallBindings()
   {
      Container.Bind<RequestQueue>().AsSingle();
      Container.BindInterfacesAndSelfTo<SfxService>().AsSingle();
      Container.BindInstance(sfxServiceConfigSo);
      Container.DeclareSignal<PlaySfxSignal>();
      Container.BindMemoryPool<SFXAudio, SFXAudioPool>()
               .FromComponentInNewPrefab(prefabSfxAudio);
      Container.Bind<WeatherSpriteCache>().AsSingle();
   }
}