// 日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class AudioController : MonoBehaviour
{
    [SerializeField]
    private Slider _bgmVolumeSlider = default;
    [SerializeField]
    private Slider _seVolumeSlider = default;

    private void OnEnable()
    {
        _bgmVolumeSlider?.onValueChanged.AddListener(AudioManager.ChangeBGMVolume);
        _seVolumeSlider?.onValueChanged.AddListener(AudioManager.ChangeSEVolume);
    }
    private void OnDisable()
    {
        _bgmVolumeSlider?.onValueChanged.RemoveListener(AudioManager.ChangeBGMVolume);
        _seVolumeSlider?.onValueChanged.RemoveListener(AudioManager.ChangeSEVolume);
    }
}
