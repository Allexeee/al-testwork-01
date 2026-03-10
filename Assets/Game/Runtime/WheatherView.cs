using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WeatherView : MonoBehaviour
{
   public GameObject loader;

   private readonly List<WeatherItemView> _items = new();

   [Inject] private readonly WheatherItemViewPool _itemPool;
   [Inject] private readonly SignalBus            _signalBus;

   public void Clear()
   {
      foreach (var item in _items)
         _itemPool.Despawn(item);

      _items.Clear();
   }

   public void AddItem(int number, string iconUrl, string label, string temp)
   {
      var item = _itemPool.Spawn(number, iconUrl, label, temp);
      _items.Add(item);
   }

   public void ShowLoader(bool show = true)
   {
      loader.SetActive(show);
   }
}