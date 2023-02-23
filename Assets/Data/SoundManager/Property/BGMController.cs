// 日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController
{
    public void Setup()
    {

    }
    /// <summary>
    /// BGMのボリュームを調整する。
    /// </summary>
    /// <param name="ratio">0に近いほど小さく、1に近づくほど大きくなる。</param>
    public void VolumeAdjustment(float ratio)
    {
        ratio = Mathf.Clamp(ratio, 0f, 1f);
    }
    public void ChangeAudioClip(AudioClip clip)
    {

    }
}
