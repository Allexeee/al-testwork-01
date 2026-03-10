using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DogItemView : MonoBehaviour
{
   public TMP_Text   _label;
   public GameObject _loader;
   public Button     _button;

   private Action _onClick;

   public void Init(string labelText, Action onClick)
   {
      _label.text = labelText;
      _onClick    = onClick;
      ShowLoader(false);
   }

   private void Click()
   {
      _onClick?.Invoke();
   }

   public void ShowLoader(bool val)
   {
      _loader.SetActive(val);
   }

   void OnEnable()
   {
      _button.onClick.AddListener(Click);
   }

   void OnDisable()
   {
      _button.onClick.RemoveListener(Click);
   }
}

public class DogItemViewPool : MemoryPool<int, string, Action, DogItemView>
{
   protected override void Reinitialize(int p0, string p1, Action p2, DogItemView item)
   {
      item.transform.SetSiblingIndex(p0);
      item.Init(p1, p2);
   }

   protected override void OnSpawned(DogItemView item)
   {
      item.gameObject.SetActive(true);
   }

   protected override void OnDespawned(DogItemView item)
   {
      if (item)
         item.gameObject.SetActive(false);
   }
}