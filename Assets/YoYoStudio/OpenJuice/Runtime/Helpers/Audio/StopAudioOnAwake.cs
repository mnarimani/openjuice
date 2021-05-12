// Copyright (c) 2020 Omid Saadat (@omid3098)
#if !AUDOTY
using UnityEngine;

namespace YoYoStudio.OpenJuice
{
    public class StopAudioOnAwake : MonoBehaviour
    {
        [SerializeField] AudioClip clip = null;
        private void Awake() => Juicer.Instance.StopSFX(clip);
    }
}
#endif