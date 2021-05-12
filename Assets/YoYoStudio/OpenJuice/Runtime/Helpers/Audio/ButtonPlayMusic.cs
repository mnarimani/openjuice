// Copyright (c) 2020 Omid Saadat (@omid3098)
#if !AUDOTY
using UnityEngine;

namespace YoYoStudio.OpenJuice
{
    public class ButtonPlayMusic : BaseButton
    {
        [SerializeField] AudioClip clip = null;
        protected override void OnPressed() => Juicer.Instance.PlayMusic(clip);
    }
}
#endif
