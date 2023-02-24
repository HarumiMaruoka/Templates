// 日本語対応
using UnityEngine;
using UniRx;

public class SEPlayer : MonoBehaviour
{
    private AudioSource _audioSource = null;

    public AudioSource AudioSource => _audioSource;

    private void Awake()
    {
        _audioSource = this.gameObject.AddComponent<AudioSource>();
        AudioManager.SEVolume.Subscribe(value => _audioSource.volume = value);
    }
}
