using DG.Tweening;
using UnityEngine;

namespace YoYoStudio.OpenJuice
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeTransition : BaseTransition
    {
        private CanvasGroup canvasGroup;

        protected override void MakeTweens()
        {
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();

            {
                Sequence seq = DOTween.Sequence();
                seq.AppendCallback(() => canvasGroup.alpha = 0);
                seq.Append(canvasGroup.DOFade(1, Duration));
                seq.SetDelay(Delay).SetLoops(Loop, LoopType).SetAutoKill(false);
                tween = seq;
                tween.Pause();
            }

            {
                Sequence seq = DOTween.Sequence();
                seq.AppendCallback(() => canvasGroup.alpha = 1);
                seq.Append(canvasGroup.DOFade(0, Duration));
                seq.SetDelay(RewindDelay).SetLoops(Loop, LoopType).SetAutoKill(false);
                rewindTween = seq;
                rewindTween.Pause();
            }
        }
    }
}