// 日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BGMPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClip _initialaudioClip = default;

    private AudioSource _audioSource = null;

    public AudioSource AudioSource => _audioSource;

    private void Awake()
    {
        _audioSource = this.gameObject.AddComponent<AudioSource>();
        AudioManager.BGMVolume.Subscribe(value => _audioSource.volume = value);
        if (_initialaudioClip != null)
        {
            _audioSource.clip = _initialaudioClip;
        }
    }
    public void ChangeBGM(AudioClip audioClip)
    {
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }
}
