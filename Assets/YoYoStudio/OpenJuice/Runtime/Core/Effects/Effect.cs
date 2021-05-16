// Copyright (c) 2020 Omid Saadat (@omid3098)
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

#if AUDOTY
using Audoty;
#endif

namespace YoYoStudio.OpenJuice
{
    public class Effect : MonoBehaviour
    {
        public virtual string Id => gameObject.name;
#if AUDOTY
        [SerializeField] AudioPlayer startClip, loopClip, endClip;
#else
        [SerializeField] AudioClip startClip, loopClip, endClip = null;
#endif
        
        [SerializeField] float duration = 0f;
        
#if AUDOTY
        private AudioHandle loopHandle;
        private AudioHandle startHandle;
#else
        private AudioSource loopSource;
#endif
        private bool effectStarted;

        public virtual void PlayStartEffect()
        {
            if (startClip != null)
            {
#if AUDOTY
                startHandle = startClip.Play();
#else
                Juicer.Instance.PlaySfx(startClip, PlayLoopEffect);
#endif
            }


#if AUDOTY
            PlayLoopEffect();
#else
            if (startClip == null)
                PlayLoopEffect();
#endif
            
            effectStarted = true;
            if (duration > 0)
            {
                WaitForEffectDuration();
            }
        }

        private async void WaitForEffectDuration()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            Juicer.Instance.ReleaseEffect(this);
        }

        public virtual void PlayLoopEffect()
        {
            if (loopClip != null)
            {
#if AUDOTY
                loopHandle = loopClip.Play(delay: startHandle.ClipLength);
#else
                loopSource = Juicer.Instance.PlaySfx(loopClip, true);
#endif
            }
        }

        public virtual void PlayEndEffect()
        {
            if (endClip != null)
            {
#if AUDOTY
                endClip.Play();
#else
                Juicer.Instance.PlaySfx(endClip);
#endif
            }
        }
        
        private void OnDisable()
        {
            StopEffectSFX();
        }

        private void StopEffectSFX()
        {
            if (effectStarted)
            {
#if AUDOTY
                if (startHandle.IsPlaying())
                    startHandle.Stop();
                loopHandle.Stop();
#else
                if (loopSource != null) 
                    Juicer.Instance.StopSFX(loopSource);
#endif
                PlayEndEffect();
            }
            
            effectStarted = false;
        }
    }
}