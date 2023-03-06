// 日本語対応
using UnityEngine;

[System.Serializable]
public class SaveLoadTester2
{
#pragma warning disable 0414
    [SerializeField]
    private int a = 1;
    [SerializeField]
    private float b = 2.3f;
    [SerializeField]
    private string c = "abcde";

    [SerializeField]
    private int[] d = default;
    [SerializeField]
    private string[] e = default;
    [SerializeField]
    private SaveLoadTester3 f = default;
}
