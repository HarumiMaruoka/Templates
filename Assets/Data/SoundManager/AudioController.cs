// 日本語対応
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [SerializeField]
    private Slider _bgmVolumeSlider = default;
    [SerializeField]
    private Slider _seVolumeSlider = default;

    private void Awake()
    {
        SliderInit(_bgmVolumeSlider, AudioManager.BGMVolume.Value);
        SliderInit(_seVolumeSlider, AudioManager.SEVolume.Value);
    }
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
    /// <summary>
    /// スライダーの初期化処理
    /// </summary>
    /// <param name="slider"> 初期化対象のスライダー </param>
    /// <param name="initializeValue"> スライダーの初期値 </param>
    private void SliderInit(Slider slider, float initializeValue)
    {
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.wholeNumbers = false;
        slider.value = initializeValue;
    }
}
