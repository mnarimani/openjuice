// Copyright (c) 2020 Omid Saadat (@omid3098)

using System;
using System.Collections.Generic;
using UnityEngine;

#if UI_EFFECT
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
#endif

namespace YoYoStudio.OpenJuice
{
    public class Juicer : Singleton<Juicer>
    {
        private Dictionary<string, ObjectPool<Effect>> effectsPool;
        private List<Effect> effectPrefabs;
#if !AUDOTY
        private AudioPlayer sfxAudioPlayer;
        private AudioPlayer musicAudioPlayer;
#endif

#if UI_EFFECT
        private TransitionEffect transitionEffect;
#endif

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

#if UI_EFFECT
        public async void LoadScene(string sceneName)
        {
            if (transitionEffect == null) transitionEffect = (TransitionEffect)PlayEffect("TransitionCanvas");
            transitionEffect.Show();
            await UniTask.Delay(TimeSpan.FromSeconds(transitionEffect.Duration));
            SceneManager.LoadSceneAsync(sceneName).completed += LoadSceneComplete;
        }

        private void LoadSceneComplete(AsyncOperation obj)
        {
            transitionEffect.Hide();
        }
#endif

        private void Initialize()
        {
            effectsPool = new Dictionary<string, ObjectPool<Effect>>();
            effectPrefabs = new List<Effect>();
            LoadAllEffectPrefabs();
            CreateObjectPoolForEachEffect();
#if !AUDOTY
            sfxAudioPlayer = new AudioPlayer(transform, "sfx");
            musicAudioPlayer = new AudioPlayer(transform, "music");
#endif
        }

        private void CreateObjectPoolForEachEffect()
        {
            foreach (Effect prefab in effectPrefabs)
            {
                if (effectsPool.ContainsKey(prefab.Id))
                {
                    Debug.LogError($"[Juicer] Duplicate effects with id {prefab.Id}");
                    continue;
                }
                
                var effectPool = new ObjectPool<Effect>(1, () =>
                    {
                        var effect = Instantiate(prefab);
                        effect.name = prefab.name;
                        effect.transform.SetParent(transform, false);
                        effect.gameObject.SetActive(false);
                        return effect;
                    }, (effect) => { effect.gameObject.SetActive(true); }
                    , (effect) => { effect.gameObject.SetActive(false); });
                effectPool.WarmUp(1);

                    effectsPool.Add(prefab.Id, effectPool);
            }
        }

        private void LoadAllEffectPrefabs()
        {
            var databases = Resources.LoadAll<EffectDatabase>("");
            foreach (EffectDatabase effectDatabase in databases)
            {
                for (int i = 0; i < effectDatabase.effectLists.Count; i++)
                {
                    EffectPack item = effectDatabase.effectLists[i];
                    if (item != null && item.effects != null) effectPrefabs.AddRange(item.effects);
                }
            }
        }

        public Effect PlayEffect(string effectID)
        {
            var effect = GetEffect(effectID);
            effect.PlayStartEffect();
            return effect;
        }

        public Effect GetEffect(string effectID)
        {
            effectsPool.TryGetValue(effectID, out ObjectPool<Effect> poolEffect);
            if (poolEffect != null)
            {
                return poolEffect.Get();
            }

            Debug.LogError("No effect found with id: " + effectID);
            return null;
        }

        public void ReleaseEffect(Effect effect)
        {
            if (effect.transform.IsChildOf(transform) == false)
            {
                effect.transform.SetParent(transform, false);
            }

            effectsPool.TryGetValue(effect.Id, out ObjectPool<Effect> poolEffect);
            if (poolEffect != null)
            {
                poolEffect.Release(effect);
            }
            else
            {
                Debug.LogError("No pool was found for effect: " + effect.Id);
            }
        }

#if !AUDOTY
        public AudioSource PlaySfx(AudioClip clip, bool loop = false) => sfxAudioPlayer.Play(clip, loop);
        public AudioSource PlaySfx(AudioClip clip, Action onComplete) => sfxAudioPlayer.Play(clip, onComplete);
        public AudioSource PlayMusic(AudioClip clip, bool loop = true) => musicAudioPlayer.Play(clip, loop);
        public AudioSource PlayMusic(AudioClip clip, Action onComplete) => musicAudioPlayer.Play(clip, onComplete);
        public void StopSFX(AudioClip clip) => sfxAudioPlayer.ReleaseSource(clip);
        public void StopSFX(AudioSource audioSource) => sfxAudioPlayer.ReleaseSource(audioSource);
        public void StopMusic(AudioClip clip) => musicAudioPlayer.ReleaseSource(clip);
        public void StopMusic(AudioSource audioSource) => musicAudioPlayer.ReleaseSource(audioSource);
#endif
    }
}
