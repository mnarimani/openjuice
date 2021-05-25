// Copyright (c) 2020 Omid Saadat (@omid3098)

using System.Threading;
using DG.Tweening;
using UnityEngine;
using Cysharp.Threading.Tasks;

#if !UNITASK_DOTWEEN_SUPPORT
using System;

#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#elif NAUGHTY_ATTRIBUTES
using NaughtyAttributes;
#endif

namespace YoYoStudio.OpenJuice
{
    public abstract class BaseTransition : MonoBehaviour
    {
        [SerializeField] private float duration = 0.4f;
        [SerializeField] private float rewindDuration = 0.4f;
        [SerializeField] private bool playOnEnable = true;
        [SerializeField] private float delay = 0;
        [SerializeField] private float rewindDelay = 0;
        [SerializeField] private Ease easeType = Ease.OutQuart;
        [SerializeField] private TransitionType transitionType = TransitionType.To;
        [Tooltip("-1 for infinite loop"), SerializeField] private int loop = 1;
        [SerializeField] private LoopType loopType = LoopType.Yoyo;
        [SerializeField] private bool relative = true;
        [SerializeField] private bool localSpace = false;

        protected Tween tween, rewindTween;

        public float Duration { get => duration; set => duration = value; }
        public float RewindDuration { get => rewindDuration; set => rewindDuration = value; }
        public float Delay { get => delay; set => delay = value; }
        public float RewindDelay { get => rewindDelay; set => rewindDelay = value; }
        public Ease EaseType { get => easeType; set => easeType = value; }
        public TransitionType TransitionType { get => transitionType; set => transitionType = value; }
        public int Loop { get => loop; set => loop = value; }
        public LoopType LoopType { get => loopType; set => loopType = value; }
        public bool Relative { get => relative; set => relative = value; }
        public bool LocalSpace { get => localSpace; set => localSpace = value; }
        public bool PlayOnEnable { get => playOnEnable; set => playOnEnable = value; }

        private void OnEnable()
        {
            if (PlayOnEnable)
                Play();
        }

        private void OnDestroy()
        {
            if (tween != null && tween.IsActive())
                tween.Kill();
            
            if (rewindTween != null && rewindTween.IsActive())
                rewindTween.Kill();
        }

#if ODIN_INSPECTOR || NAUGHTY_ATTRIBUTES
        [Button("Play")]
#endif
        public void Play()
        {
            Play(gameObject.GetCancellationTokenOnDestroy());
        }

#if ODIN_INSPECTOR || NAUGHTY_ATTRIBUTES
        [Button("Play Reverse")]
#endif
        private void PlayReverse()
        {
            PlayReverse(gameObject.GetCancellationTokenOnDestroy());
        }
        
        public void Play(CancellationToken? ct)
        {
            if (rewindTween != null && rewindTween.IsActive() && rewindTween.IsPlaying())
                rewindTween.Pause();

            if (tween != null && tween.IsActive())
            {
                if (tween.IsPlaying())
                    return;

                // We don't want to cache tween in editor to enable editing parameters while in play mode.
#if !UNITY_EDITOR
                tween.Restart();
                return;
#endif
            }

            MakeTweens();
            
            tween.Play();

            if (ct != null)
            {
#if UNITASK_DOTWEEN_SUPPORT
                tween.WithCancellation(ct.Value);
#else
                throw UniTaskDoTweenNeededException();          
#endif
            }
        }
        
        public void PlayReverse(CancellationToken? ct)
        {
            if (tween != null && tween.IsActive() && tween.IsPlaying())
                tween.Pause();
            
            if (rewindTween != null && rewindTween.IsActive())
            {
                if (rewindTween.IsPlaying())
                    return;

#if !UNITY_EDITOR
                rewindTween.Restart();
                return;
#endif
            }
            
            MakeTweens();
            rewindTween.Play();
            
            if (ct != null)
            {
#if UNITASK_DOTWEEN_SUPPORT
                rewindTween.WithCancellation(ct.Value);
#else
                throw UniTaskDoTweenNeededException();          
#endif
            }
        }

        public UniTask PlayAsync(CancellationToken? ct = null)
        {
            Play(ct);
#if UNITASK_DOTWEEN_SUPPORT
            return tween.AwaitForComplete();
#else
            return UniTaskDoTweenNeededAsync();
#endif
        }

        public UniTask PlayReverseAsync(CancellationToken? ct = null)
        {
            PlayReverse(ct);
#if UNITASK_DOTWEEN_SUPPORT
            return rewindTween.AwaitForComplete();
#else
            return UniTaskDoTweenNeededAsync();
#endif
        }

#if !UNITASK_DOTWEEN_SUPPORT
        private static UniTask UniTaskDoTweenNeededAsync()
        {
            return UniTask.FromException(UniTaskDoTweenNeededException());
        }

        private static Exception UniTaskDoTweenNeededException()
        {
            return new NotSupportedException("You need to have UniTask for DoTween enabled. Add UNITASK_DOTWEEN_SUPPORT to Scripting Define Symbols in Player Settings");
        }
#endif

        protected abstract void MakeTweens();
    }
}