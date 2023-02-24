// 日本語対応
using UnityEngine;

public class Rotate2 : MonoBehaviour
{
    private float _deg = 0f;
    private float _deg2 = 0f;

    [Tooltip("x,z平面上を回転する速さ"), SerializeField]
    private float _rotateSpeed = 80f;
    [Tooltip("x,z平面上を回転する半径"), SerializeField]
    private float _xzRadius = 5f;

    [Tooltip("一周あたり何回上下するかを表す値"), SerializeField]
    private float _waveCount = 6f;
    [SerializeField]
    private float _waveSize = 2f;

    private void Update()
    {
        transform.position =
            new Vector3(
                Mathf.Cos(_deg) * (_xzRadius + _waveSize * Mathf.Sin(_deg2 * _waveCount)),
                0f,
                 Mathf.Sin(_deg) * (_xzRadius + _waveSize * Mathf.Sin(_deg2 * _waveCount)));
        // x, z平面上に回転する 
        _deg += 2f * Mathf.PI / 360f * Time.deltaTime * _rotateSpeed;
        // 縦に揺らす
        _deg2 += 2f * Mathf.PI / 360f * Time.deltaTime * _rotateSpeed;
    }
}
