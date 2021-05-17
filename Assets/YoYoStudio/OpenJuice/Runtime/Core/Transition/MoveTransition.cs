// Copyright (c) 2020 Omid Saadat (@omid3098)
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
namespace YoYoStudio.OpenJuice
{
    public class MoveTransition : BaseTransition
    {
        [SerializeField] Vector3 targetPosition = Vector3.zero;

        public Vector3 TargetPosition
        {
            get => targetPosition; set
            {
                targetPosition = value;
                MakeTweens();
            }
        }

        protected override void MakeTweens()
        {
            Vector3 originalPosition = LocalSpace ? transform.localPosition : transform.position;

            {
                Tweener forward = LocalSpace
                    ? transform.DOLocalMove(targetPosition, Duration)
                    : transform.DOMove(targetPosition, Duration);
                forward.SetEase(EaseType).SetLoops(Loop, LoopType).SetDelay(Delay).SetAutoKill(false);

                if (TransitionType == TransitionType.From)
                    forward.From(Relative);
                else
                    forward.SetRelative(Relative);

                tween = forward;
                tween.Pause();
            }


            {
                Vector3 rewindPosition = Relative ? targetPosition * -1 : originalPosition;
                Tweener backward = LocalSpace ? transform.DOLocalMove(rewindPosition, RewindDuration) : transform.DOMove(rewindPosition, RewindDuration);
                backward.SetEase(EaseType).SetLoops(Loop, LoopType).SetDelay(RewindDelay).SetAutoKill(false);
                if (TransitionType == TransitionType.From)
                    backward.From(Relative);
                else
                    backward.SetRelative(Relative);
                
                rewindTween = backward;
                rewindTween.Pause();
            }
        }
    }
}