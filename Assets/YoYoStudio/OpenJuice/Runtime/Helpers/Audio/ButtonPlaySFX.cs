﻿// Copyright (c) 2020 Omid Saadat (@omid3098)
#if !AUDOTY
using UnityEngine;

namespace YoYoStudio.OpenJuice
{
    public class ButtonPlaySFX : BaseButton
    {
        [SerializeField] AudioClip clip = null;
        protected override void OnPressed() => Juicer.Instance.PlaySfx(clip);
    }
}
#endif