using DG.Tweening;
using UnityEngine;

namespace YoYoStudio.OpenJuice
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeTransition : BaseTransition
    {
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        protected override void MakeTweens()
        {
            tween = canvasGroup.DOFade(1, Duration).SetDelay(Delay).SetAutoKill(false);
            rewindTween = canvasGroup.DOFade(0, RewindDuration).SetDelay(RewindDelay).SetAutoKill(false);
        }
    }
}