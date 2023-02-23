public interface IState
{
    /// <summary> このステートに遷移したとき一度だけ実行する処理 </summary>
    public void Enter();
    /// <summary> このステートから他のステートへ遷移するとき一度だけ実行する処理 </summary>
    public void Exit();
    /// <summary> このステートの振る舞い </summary>
    public void Behavior()
    {
        if (Transition()) return;
        else Update();
    }
    /// <summary> 遷移処理を記述する。 </summary>
    /// <returns> 遷移する時 true, そうでないとき falseを返す。 </returns>
    public bool Transition();
    /// <summary> 毎フレーム実行する処理 </summary>
    public void Update();
}