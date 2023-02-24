// 日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    private float _deg = 0f;
    private float _deg2 = 0f;

    [Tooltip("x,z平面上を回転する速さ"), SerializeField]
    private float _rotateSpeed = 80f;
    [Tooltip("x,z平面上を回転する半径"), SerializeField]
    private float _xzRadius = 5f;

    [Tooltip("一周あたり何回上下するかを表す値"), SerializeField]
    private float _upDownNumber = 6f;
    [Tooltip("上下の振れ幅"), SerializeField]
    private float _yRadius = 1.5f;

    private void Update()
    {
        transform.position =
            new Vector3(
                _xzRadius * Mathf.Cos(_deg),
                _yRadius * Mathf.Sin(_deg2),
                _xzRadius * Mathf.Sin(_deg));
        // x, z平面上に回転する 
        _deg += 2f * Mathf.PI / 360f * Time.deltaTime * _rotateSpeed;
        // 縦に揺らす
        _deg2 += Time.deltaTime * _upDownNumber;
    }
}
