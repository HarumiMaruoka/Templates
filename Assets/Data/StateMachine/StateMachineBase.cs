using System;
using UnityEngine;

[System.Serializable]
public abstract class StateMachineBase<TEnumeration, TState>
    where TEnumeration : Enum
    where TState : class, IState
{
    /// <summary>
    /// 現在のステートを表すオブジェクト
    /// </summary>
    private TState _currentState = null;
    /// <summary>
    /// ステートが変更された時に実行するデリゲート。<br/>
    /// 第一引数に遷移前のステート。第二引数に遷移後のステートが渡される。
    /// </summary>
    public event Action<TState, TState> OnStateChanged = default;
    /// <summary>
    /// 初期化済みかどうか表す値
    /// </summary>
    private bool _isInit = false;

    public void Update()
    {
        if (_currentState != null)
        {
            _currentState.Behavior();
        }
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="initializeState">
    /// 最初のステート
    /// </param>
    /// <param name="holder">
    /// 各ステートを保持/提供するオブジェクト
    /// </param>
    public void Init(TState initializeState)
    {
        if (!_isInit)
        {

#if UNITY_EDITOR
            OnStateChanged +=
                (previousState, newState) =>
                {
                    Debug.Log(
                        $"プレイヤーのステートが変更されました。\n" +
                        $"現在のステートは\"{newState.GetType().Name}\"です。");
                };
#endif

            _isInit = true;
            _currentState = initializeState;
            _currentState.Enter();
            OnStateChanged?.Invoke(null, _currentState);
        }
    }

    // ステートの遷移処理。引数に「次のステートの参照」を受け取る。
    public void TransitionTo(TState nextState)
    {
        if (nextState == null)
        {
            Debug.LogError($"引数に渡された {nextState} はnullです。遷移をキャンセルします。");
            return;
        }
        var previousState = _currentState; // 変更前のステートを保存
        _currentState.Exit();              // 現在ステートの終了処理。
        _currentState = nextState;         // 現在のステートの変更処理。
        nextState.Enter();                 // 変更された「新しい現在ステート」のEnter処理。

        // ステート変更時のアクションを実行する。
        // 引数に「新しい現在ステート」を渡す。
        OnStateChanged?.Invoke(previousState, nextState);
    }
}