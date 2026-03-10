using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherService
{
   private const string ApiUrl = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";

   public async UniTask<List<DailyWeather>> GetWeatherAsync(System.Threading.CancellationToken token)
   {
      using var req = UnityWebRequest.Get(ApiUrl);
      await req.SendWebRequest().WithCancellation(token);

      if (req.result != UnityWebRequest.Result.Success)
         throw new System.Exception(req.error);

      var json = req.downloadHandler.text;

      var forecast = JsonUtility.FromJson<WeatherForecastResponse>(json);

      return WeatherPeriodGrouper.GroupByDay(forecast.properties.periods.ToList());
   }
}

[Serializable]
public class WeatherForecastResponse
{
   public WeatherProperties properties;
}

[Serializable]
public class WeatherProperties
{
   public WeatherPeriod[] periods;
}

[Serializable]
public class WeatherPeriod
{
   public int    number;
   public string name;
   public string startTime;
   public int    temperature;
   public string temperatureUnit;
   public string icon;
   public string shortForecast;
   public string detailedForecast;
}

public class WeatherDayPart
{
   public string label; // "Утро", "День", "Вечер", "Ночь"
   public string temp;
   public string iconUrl;
}

public class DailyWeather
{
   public string               dayName; // "Сегодня", "Завтра" или "Пт" и т.д.
   public List<WeatherDayPart> parts;   // Утро/день/вечер/ночь
}

public static class WeatherPeriodGrouper
{
   public static List<DailyWeather> GroupByDay(List<WeatherPeriod> periods)
   {
      var outList   = new List<DailyWeather>();
      var dailyDict = new Dictionary<string, DailyWeather>();

      foreach (var p in periods)
      {
         var date    = DateTime.Parse(p.startTime, null, DateTimeStyles.RoundtripKind);
         var dayKey  = date.ToString("yyyy-MM-dd");
         var dayName = GetDayLabel(date);

         if (!dailyDict.ContainsKey(dayKey))
         {
            dailyDict[dayKey] = new DailyWeather{
               dayName = dayName,
               parts   = new List<WeatherDayPart>()
            };

            outList.Add(dailyDict[dayKey]);
         }

         // Определяем часть суток
         var partLabel = GetPartOfDay(date);

         dailyDict[dayKey].parts.Add(new WeatherDayPart{
            label   = partLabel,
            temp    = $"{p.temperature}°{p.temperatureUnit}",
            iconUrl = p.icon,
         });
      }

      return outList;
   }

   private static string GetDayLabel(DateTime date)
   {
      var now = DateTime.Now;
      if (date.Date == now.Date) return "Сегодня";
      if (date.Date == now.AddDays(1).Date) return "Завтра";
      return date.ToString("ddd", new CultureInfo("ru-RU"));
   }

   private static string GetPartOfDay(DateTime dt)
   {
      var hour = dt.Hour;
      if (hour >= 6  && hour < 12) return "Утро";
      if (hour >= 12 && hour < 18) return "День";
      if (hour >= 18 && hour < 23) return "Вечер";
      return "Ночь";
   }
}

public class WeatherSpriteCache
{
   private readonly Dictionary<string, Sprite> _cache = new();

   public void Clear()
   {
      _cache.Clear();
   }

   public async UniTask<Sprite> GetOrLoad(string url)
   {
      if (string.IsNullOrEmpty(url))
         return null;

      if (_cache.TryGetValue(url, out var sprite))
         return sprite;

      var newSprite = await LoadSpriteFromUrl(url);
      if (newSprite != null)
         _cache[url] = newSprite;

      return newSprite;
   }

   private static async UniTask<Sprite> LoadSpriteFromUrl(string url)
   {
      using var uwr = UnityWebRequestTexture.GetTexture(url);
      await uwr.SendWebRequest();

      if (uwr.result != UnityWebRequest.Result.Success)
      {
         Debug.LogWarning($"Failed to download weather icon: {url}");
         return null;
      }

      var tex = DownloadHandlerTexture.GetContent(uwr);
      return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);
   }
}