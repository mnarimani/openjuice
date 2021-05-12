// Copyright (c) 2020 Omid Saadat (@omid3098)

using DG.Tweening;
using UnityEngine;

namespace YoYoStudio.OpenJuice
{
    public class ScaleTransition : BaseTransition
    {
        [SerializeField] Vector3 targetScale = Vector3.one;

        public Vector3 TargetScale
        {
            get => targetScale;
            set
            {
                targetScale = value;
                if (tween != null) ((Tweener) tween).ChangeEndValue(targetScale);
            }
        }

        protected override void MakeTweens()
        {
            tween = transform.DOScale(TargetScale, Duration).SetEase(EaseType).SetLoops(Loop, LoopType).SetDelay(Delay).SetAutoKill(false);

            if (TransitionType == TransitionType.From)
                ((Tweener) tween).From(Relative);
            else
                tween.SetRelative(Relative);

            tween.Pause();
        }
    }
}