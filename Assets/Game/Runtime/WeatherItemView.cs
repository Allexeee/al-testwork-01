using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class WeatherItemView : MonoBehaviour
{
   public Image    _icon;
   public TMP_Text _day;
   public TMP_Text _temp;

   [Inject] private WeatherSpriteCache _spriteCache;

   public async UniTaskVoid Init(string iconUrl, string label, string temper)
   {
      _day.text    = label;
      _temp.text   = temper;
      _icon.sprite = await _spriteCache.GetOrLoad(iconUrl).AttachExternalCancellation(this.GetCancellationTokenOnDestroy());
   }
}

public class WheatherItemViewPool : MemoryPool<int, string, string, string, WeatherItemView>
{
   protected override void Reinitialize(int p0, string p1, string p2, string p3, WeatherItemView item)
   {
      item.transform.SetSiblingIndex(p0);
      item.Init(p1, p2, p3).Forget();
   }

   protected override void OnSpawned(WeatherItemView item)
   {
      item.gameObject.SetActive(true);
   }

   protected override void OnDespawned(WeatherItemView item)
   {
      if (item)
         item.gameObject.SetActive(false);
   }
}