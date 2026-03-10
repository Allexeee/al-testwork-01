using UnityEngine;

public static class UnityExtensions
{
   public static void SetXY(this Transform transform, Vector2 vector2)
   {
      var pos = transform.position;
      pos.x              = vector2.x;
      pos.y              = vector2.y;
      transform.position = pos;
   }  
   
   public static void SetXY(this Transform transform, float x, float y)
   {
      var pos = transform.position;
      pos.x              = x;
      pos.y              = y;
      transform.position = pos;
   }
}