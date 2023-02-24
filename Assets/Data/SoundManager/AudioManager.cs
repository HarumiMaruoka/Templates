// 日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class AudioManager
{
    private static ReactiveProperty<float> _bgmVolume = new ReactiveProperty<float>(1f);
    private static ReactiveProperty<float> _seVolume = new ReactiveProperty<float>(1f);

    public static IReadOnlyReactiveProperty<float> BGMVolume => _bgmVolume;
    public static IReadOnlyReactiveProperty<float> SEVolume => _seVolume;

    public static void ChangeBGMVolume(float value)
    {
        _bgmVolume.Value = Mathf.Clamp01(value);
    }
    public static void ChangeSEVolume(float value)
    {
        _seVolume.Value = Mathf.Clamp01(value);
    }
}
