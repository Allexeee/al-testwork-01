using System;
using Zenject;
using System.Threading;
using Cysharp.Threading.Tasks;

public class DogTabPresenter : IInitializable, IDisposable
{
   private readonly DogTabView    _view;
   private readonly DogApiService _service;
   private readonly SignalBus     _signalBus;
   private readonly RequestQueue  _queue;

   private CancellationTokenSource _requestCts;

   public DogTabPresenter(DogTabView view, DogApiService service, SignalBus signalBus, RequestQueue queue)
   {
      _view      = view;
      _service   = service;
      _signalBus = signalBus;
      _queue     = queue;
   }

   public void Initialize()
   {
      _signalBus.Subscribe<ShowTabSignal>(OnShowTab);
      _view._popup.Hide();
   }

   public void Dispose()
   {
      _signalBus.Unsubscribe<ShowTabSignal>(OnShowTab);
   }

   private void OnShowTab(ShowTabSignal s)
   {
      if (s.Tab == TabType.DogBreeds)
      {
         ShowTab();
      }
      else
      {
         _queue.Remove(_requestCts);
         _requestCts = default;
      }
   }

   void ShowTab()
   {
      _view.Clear();
      _view.ShowLoader(true);

      _queue.Remove(_requestCts);
      _queue.Enqueue(out _requestCts, async ct =>
      {
         var breeds = await _service.GetBreedsAsync(ct);
         _view.Clear();
         for (var i = 0; i < breeds.Length && i < 10; i++)
         {
            var name  = breeds[i].attributes?.name ?? "Breed";
            var label = $"{i + 1} - {name}";
            var id    = breeds[i].id;

            _view.AddItem(id, i, label, () => OnClick(id));
            await UniTask.WaitForSeconds(0.1f, cancellationToken: ct);
         }

         _view.ShowLoader(false);
      });
   }

   void OnClick(string id)
   {
      _view.PlaySFXClick();

      var item = _view.GetItemByKey(id);
      item.ShowLoader(true);

      _queue.Remove(_requestCts);
      _queue.Enqueue(out _requestCts, async ct =>
      {
         var data = await _service.GetBreedFactAsync(id, ct);

         item.ShowLoader(false);
         _view._popup.Show(data.attributes.name, data.attributes.description, OnClickHidePopup);
      });
   }

   void OnClickHidePopup()
   {
      _view.PlaySFXClick();
      _view._popup.Hide();
   }
}