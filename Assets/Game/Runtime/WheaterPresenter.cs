using System;
using System.Threading;
using Zenject;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class WeatherPresenter : IInitializable, IDisposable
{
   private readonly WeatherView             _view;
   private readonly WeatherService          _service;
   private readonly SignalBus               _signalBus;
   private readonly RequestQueue            _requestQueue;
   private          CancellationTokenSource _timerCts;

   private bool _isFirstUpdate;

   public WeatherPresenter(WeatherView view, WeatherService service, SignalBus signalBus, RequestQueue requestQueue)
   {
      _view         = view;
      _service      = service;
      _signalBus    = signalBus;
      _requestQueue = requestQueue;
   }

   public void Initialize()
   {
      _signalBus.Subscribe<ShowTabSignal>(OnShowTab);
   }

   public void Dispose()
   {
      _signalBus.Unsubscribe<ShowTabSignal>(OnShowTab);
      StopWeatherLoop();
   }

   private void OnShowTab(ShowTabSignal signal)
   {
      if (signal.Tab != TabType.Weather)
      {
         StopWeatherLoop();
         return;
      }

      StopWeatherLoop();
      StartWeatherLoop();
   }

   private void StartWeatherLoop()
   {
      StopWeatherLoop();
      _timerCts = new CancellationTokenSource();
      WeatherLoop(_timerCts.Token).Forget();
   }

   private void StopWeatherLoop()
   {
      _view.Clear();
      _requestQueue.Remove(_timerCts);
   }

   private async UniTaskVoid WeatherLoop(CancellationToken token)
   {
      _isFirstUpdate = true;

      while (!token.IsCancellationRequested)
      {
         var tcs        = new UniTaskCompletionSource();
         var showLoader = _isFirstUpdate;
         _requestQueue.Enqueue(token, async ct =>
         {
            await FetchWeather(ct, showLoader);
            tcs.TrySetResult();
         });

         await tcs.Task;

         await UniTask.Delay(TimeSpan.FromSeconds(5), cancellationToken: token);

         _isFirstUpdate = false;
      }
   }

   private async UniTask FetchWeather(CancellationToken token, bool showLoader = true)
   {
      _view.ShowLoader(showLoader);

      Debug.Log($"[Weather] Гружу информацию о погоде");

      var wheather = await _service.GetWeatherAsync(token);
      _view.Clear();

      var i = 0;
      foreach (var dailyWeather in wheather)
      {
         foreach (var part in dailyWeather.parts)
         {
            _view.AddItem(i, part.iconUrl, $"{dailyWeather.dayName} - {part.label}", part.temp);
            i++;

            if (showLoader)
               await UniTask.WaitForSeconds(0.1f, cancellationToken: token);
         }
      }

      _view.ShowLoader(false);
   }
}