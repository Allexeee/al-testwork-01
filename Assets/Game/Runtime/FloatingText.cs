using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

public class FloatingText : MonoBehaviour
{
   [SerializeField] private TMP_Text    text;
   [SerializeField] private float       floatDistance = 1;
   [SerializeField] private float       duration      = 0.6f;
   [SerializeField] private CanvasGroup canvasGroup;

   private FloatingTextPool _pool;

   [Inject]
   public void Construct(FloatingTextPool pool)
   {
      _pool = pool;
   }

   public void Show(string textValue, Vector2 position)
   {
      text.text          = textValue;
      transform.position = position;
      canvasGroup.alpha  = 1f;
      PlayAnimation();
   }

   private void PlayAnimation()
   {
      var seq = DOTween.Sequence();
      seq.Append(transform.DOMoveY(transform.position.y + floatDistance, duration).SetEase(Ease.OutCubic));
      seq.Join(canvasGroup.DOFade(0, duration));
      seq.OnComplete(() => _pool.Despawn(this));
   }
}

public class FloatingTextPool : MonoMemoryPool<string, Vector2, FloatingText>
{
   protected override void Reinitialize(string value, Vector2 position, FloatingText item)
   {
      item.Show(value, position);
   }
}