// Copyright (c) 2020 Omid Saadat (@omid3098)

using DG.Tweening;
using UnityEngine;

namespace YoYoStudio.OpenJuice
{
    public class RotateTransition : BaseTransition
    {
        [SerializeField] Vector3 targetRotation = Vector3.zero;

        public Vector3 TargetRotation
        {
            get => targetRotation;
            set
            {
                targetRotation = value;
                if (tween != null) ((Tweener) tween).ChangeEndValue(targetRotation);
            }
        }

        protected override void MakeTweens()
        {
            Quaternion original = transform.localRotation;
            
            {
                tween = transform.DOLocalRotate(TargetRotation, Duration)
                    .SetEase(EaseType)
                    .SetLoops(Loop, LoopType)
                    .SetDelay(Delay)
                    .SetAutoKill(false);
                
                if (TransitionType == TransitionType.From)
                    ((Tweener) tween).From(Relative);
                else
                    tween.SetRelative(Relative);
                
                tween.Pause();
            }

            {
                rewindTween = transform.DOLocalRotateQuaternion(original, RewindDuration)
                    .SetEase(EaseType)
                    .SetLoops(Loop, LoopType)
                    .SetDelay(RewindDelay)
                    .SetAutoKill(false);

                if (TransitionType == TransitionType.From)
                    ((Tweener) tween).From(Relative);
                else
                    tween.SetRelative(Relative);
                rewindTween.Pause();
            }
        }
    }
}