using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class SfxService : IInitializable, IDisposable
{
   Dictionary<SFXEnum, SFXAudioConfigSO> _configs = new();

   SignalBus          _signalBus;
   SFXServiceConfigSO _configSo;
   SFXAudioPool       _sfxAudioPool;

   public SfxService(SignalBus signalBus, SFXServiceConfigSO configSo, SFXAudioPool sfxAudioPool)
   {
      _signalBus    = signalBus;
      _configSo     = configSo;
      _sfxAudioPool = sfxAudioPool;
   }

   public void Initialize()
   {
      foreach (var configSo in _configSo.data)
      {
         _configs.Add(configSo.kind, configSo.elements);
      }

      _signalBus.Subscribe<PlaySfxSignal>(OnPlaySignal);
   }

   public void Dispose()
   {
      _signalBus.Unsubscribe<PlaySfxSignal>(OnPlaySignal);
   }

   void OnPlaySignal(PlaySfxSignal obj)
   {
      var data = _configs.GetValueOrDefault(obj.kind);

      if (data == default) return;

      var clip   = data.clips[Random.Range(0, data.clips.Count)];
      var volume = Random.Range(data.volumeMin, data.volumeMax);
      var pitch  = Random.Range(data.pitchMin,  data.pitchMax);
      var a      = _sfxAudioPool.Spawn(clip, volume, pitch);
      a.transform.position = obj.position;
   }
}

public class PlaySfxSignal
{
   public SFXEnum kind;
   public Vector2 position;

   public PlaySfxSignal(SFXEnum kind)
   {
      this.kind = kind;
   }

   public PlaySfxSignal(SFXEnum kind, Vector2 position)
   {
      this.kind     = kind;
      this.position = position;
   }
}