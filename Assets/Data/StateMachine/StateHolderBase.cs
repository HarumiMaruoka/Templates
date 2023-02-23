using System;
using System.Collections.Generic;

/// <summary>
/// 各ステートを保持するクラス
/// </summary>
[Serializable]
public abstract class StateHolderBase<TEnumeration, TState>
    where TEnumeration : Enum
    where TState : IState
{
    /// <summary>
    /// 各ステートを保有するオブジェクト
    /// </summary>
    protected Dictionary<TEnumeration, TState> _states = new Dictionary<TEnumeration, TState>();
    /// <summary>
    /// 各ステートを保有/提供するオブジェクト
    /// </summary>
    public IReadOnlyDictionary<TEnumeration, TState> States => _states;
}