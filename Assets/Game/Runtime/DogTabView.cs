using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DogTabView : MonoBehaviour
{
   [SerializeField] public  PopupView   _popup;
   [SerializeField] private GameObject  _loader;

   private readonly Dictionary<string, DogItemView> _items = new();

   [Inject] private readonly DogItemViewPool _itemPool;
   [Inject] private readonly SignalBus _signalBus;
   
   public void Clear()
   {
      foreach (var (_, item) in _items) 
         _itemPool.Despawn(item);
      _items.Clear();
   }

   public void AddItem(string key, int number, string name, Action onClick)
   {
      var item = _itemPool.Spawn(number, name, onClick);
      _items.Add(key, item);
   }
   
   public void ShowLoader(bool visible) => _loader.SetActive(visible);
   
   public DogItemView GetItemByKey(string key)
   {
      return _items[key];
   }

   public void PlaySFXClick() => _signalBus.Fire(new PlaySfxSignal(SFXEnum.Click));
}