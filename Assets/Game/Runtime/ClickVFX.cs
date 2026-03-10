using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class ClickVFX : MonoBehaviour
{
   [SerializeField] ParticleSystem _particleSystem;
   [SerializeField] float          _despawn;

   public async void Play(Vector2 position, Action onFinished)
   {
      transform.SetXY(position);
      _particleSystem.Play();

      await UniTask.WaitForSeconds(_despawn);

      onFinished?.Invoke();
   }
}

public class ClickVFXPool : MonoMemoryPool<ClickVFX>
{
   protected override void OnDespawned(ClickVFX item)
   {
      item.gameObject.SetActive(false);
   }

   protected override void OnSpawned(ClickVFX item)
   {
      item.gameObject.SetActive(true);
   }
}