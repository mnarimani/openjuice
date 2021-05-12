// Copyright (c) 2020 Omid Saadat (@omid3098)
using DG.Tweening;
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
            
            tween = LocalSpace ? transform.DOLocalMove(targetPosition, Duration) : transform.DOMove(targetPosition, Duration);
            
            tween.SetEase(EaseType).SetLoops(Loop, LoopType).SetDelay(Delay).SetAutoKill(false);
            
            if (TransitionType == TransitionType.From) 
                tween.From(Relative);
            else
                tween.SetRelative(Relative);
            tween.Pause();


            Vector3 rewindPosition = Relative ? targetPosition * -1 : originalPosition;
            rewindTween = LocalSpace ? transform.DOLocalMove(rewindPosition, RewindDuration) : transform.DOMove(rewindPosition, RewindDuration);
            rewindTween.SetEase(EaseType).SetLoops(Loop, LoopType).SetDelay(RewindDelay).SetAutoKill(false);
            if (TransitionType == TransitionType.From) 
                rewindTween.From(Relative);
            else
                rewindTween.SetRelative(Relative);
            rewindTween.Pause();
        }
    }
}