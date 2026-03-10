using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupView : MonoBehaviour
{
   [SerializeField] TMP_Text   _title;
   [SerializeField] TMP_Text   _description;
   [SerializeField] GameObject _root;
   [SerializeField] Button     _button;

   Action _clickHideView;

   public void Show(string name, string desc, Action clickHideView)
   {
      _title.text       = name;
      _description.text = desc;
      _root.SetActive(true);
      _clickHideView = clickHideView;
      LayoutRebuilder.ForceRebuildLayoutImmediate(_description.GetComponent<RectTransform>());
   }

   void OnEnable()
   {
      _button.onClick.AddListener(Click);
   }

   void OnDisable()
   {
      _button.onClick.RemoveListener(Click);
   }

   void Click()
   {
      _clickHideView?.Invoke();
   }

   public void Hide()
   {
      if (_root)
         _root.SetActive(false);
   }
}