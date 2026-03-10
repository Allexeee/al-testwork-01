using UnityEngine;
using Zenject;

public class TabStartup : MonoBehaviour
{
   [Inject] TabConfigSO _tabConfig;
   [Inject] SignalBus   _signalBus;

   void Start()
   {
      _signalBus.Fire(new SwitchTabSignal(_tabConfig.startTab));
   }
}