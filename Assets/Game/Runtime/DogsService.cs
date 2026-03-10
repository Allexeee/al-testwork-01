using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class BreedFactResponse
{
   public BreedEntry data;
}

[Serializable]
public class BreedsResponse
{
   public BreedEntry[] data;
}

[Serializable]
public class BreedEntry
{
   public string          id;
   public string          type;
   public BreedAttributes attributes;
}

[Serializable]
public class BreedAttributes
{
   public string name;
   public string description;
}

public class DogApiService
{
   const string API       = "https://dogapi.dog/api/v2/breeds";
   const string API_BY_ID = "https://dogapi.dog/api/v2/breeds/"; // +{id}

   public async UniTask<BreedEntry[]> GetBreedsAsync(CancellationToken token)
   {
      using var uwr = UnityWebRequest.Get(API);
      await uwr.SendWebRequest().WithCancellation(token);
      var res = JsonUtility.FromJson<BreedsResponse>(uwr.downloadHandler.text);
      return res.data;
   }

   public async UniTask<BreedEntry> GetBreedFactAsync(string id, CancellationToken token)
   {
      using var uwr = UnityWebRequest.Get($"{API_BY_ID}{id}");
      await uwr.SendWebRequest().WithCancellation(token);
      var res = JsonUtility.FromJson<BreedFactResponse>(uwr.downloadHandler.text);
      return res.data;
   }
}