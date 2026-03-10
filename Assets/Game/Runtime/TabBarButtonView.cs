using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TabBarButtonView : MonoBehaviour
{
   public   TabType   tabType;
   private  Button    _button;
   [Inject] SignalBus _signalBus;

   private void Awake()
   {
      _button = GetComponent<Button>();
      _button.onClick.AddListener(OnClick);
   }

   void OnClick()
   {
      _signalBus.Fire(new PlaySfxSignal(SFXEnum.Click));
      _signalBus.Fire(new SwitchTabSignal(tabType));
   }
}