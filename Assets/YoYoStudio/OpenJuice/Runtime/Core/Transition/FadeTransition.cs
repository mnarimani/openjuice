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
            {
                Sequence seq = DOTween.Sequence();
                seq.AppendCallback(() => canvasGroup.alpha = 0);
                seq.Append(canvasGroup.DOFade(1, Duration));
                seq.AppendCallback(() => canvasGroup.interactable = true);
                seq.SetDelay(Delay).SetLoops(Loop, LoopType).SetAutoKill(false);
                tween = seq;
                tween.Pause();
            }

            {
                Sequence seq = DOTween.Sequence();
                seq.AppendCallback(() => canvasGroup.alpha = 1);
                seq.Append(canvasGroup.DOFade(0, Duration));
                seq.AppendCallback(() => canvasGroup.interactable = false);
                seq.SetDelay(RewindDelay).SetLoops(Loop, LoopType).SetAutoKill(false);
                rewindTween = seq;
                rewindTween.Pause();
            }
        }
    }
}