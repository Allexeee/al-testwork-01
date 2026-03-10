using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class SFXAudio : MonoBehaviour
{
   [SerializeField] AudioSource _source;

   [Inject] SFXAudioPool _sfxAudioPool;

   public void Init(AudioClip clip, float volume, float pitch)
   {
      _source.clip   = clip;
      _source.volume = volume;
      _source.pitch  = pitch;
      _source.Play();

      AutoDespawnAsync(clip.length / Mathf.Abs(pitch)).Forget();
   }

   private async UniTaskVoid AutoDespawnAsync(float delay)
   {
      await UniTask.Delay((int) (delay * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
      _sfxAudioPool.Despawn(this);
   }
}

public class SFXAudioPool : MemoryPool<AudioClip, float, float, SFXAudio>
{
   protected override void Reinitialize(AudioClip p1, float p2, float p3, SFXAudio item)
   {
      item.Init(p1, p2, p3);
   }
}