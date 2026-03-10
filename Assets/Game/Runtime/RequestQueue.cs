using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class RequestQueue
{
   private class Request
   {
      public Func<CancellationToken, UniTask> Action;
      public CancellationTokenSource          Cts;
   }

   private List<Request> _queue = new();
   private bool          _processing;

   public void Enqueue(CancellationToken token, Func<CancellationToken, UniTask> action)
   {
      var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
      var req = new Request{
         Action = action,
         Cts    = cts
      };

      _queue.Add(req);
      if (!_processing)
         ProcessNext().Forget();
   }

   public void Enqueue(out CancellationTokenSource cts, Func<CancellationToken, UniTask> action)
   {
      cts = new CancellationTokenSource();
      var req = new Request{
         Action = action,
         Cts    = cts
      };

      _queue.Add(req);
      if (!_processing)
         ProcessNext().Forget();
   }

   public void Remove(CancellationTokenSource cts)
   {
      if (cts == default) return;

      cts.Cancel();

      for (var i = _queue.Count - 1; i >= 0; i--)
         if (_queue[i].Cts == cts)
            _queue.RemoveAt(i);
   }

   private async UniTaskVoid ProcessNext()
   {
      _processing = true;
      while (_queue.Count > 0)
      {
         var req = _queue[0];
         _queue.RemoveAt(0);

         if (req.Cts.IsCancellationRequested)
            continue;

         try
         {
            await req.Action(req.Cts.Token);
         }
         catch (OperationCanceledException)
         {
         }
         catch (Exception e)
         {
            Debug.LogError(e);
         }
      }

      _processing = false;
   }
}