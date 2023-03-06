using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine;
using UnityEngine.UI;

public static class SceneManager
{
    /// <summary>
    /// フェードイメージ用の描画優先度
    /// </summary>
    private const int _sortOrder = 100;
    /// <summary>
    /// フェードアウト完了時に発行するイベント。
    /// </summary>
    public static event Action OnFadeInCompleted = null;

    /// <summary>
    /// 現在稼働中のDOTween <br/>
    /// 同時に稼働できるDOTweenを一つに制限する為のフィールド
    /// </summary>
    private static TweenerCore<Color, Color, ColorOptions> _dotween = null;

    /// <summary>
    /// 起動時にFadeInを登録する。<br/>
    /// このクラスがプロジェクトに存在するだけで強制的に追加されるので <br/>
    /// 配布、複製する際は注意すること。<br/>
    /// Editor上で実行する際,Enter Play Mode Optionを使用すると予期せぬ挙動となる。<br/>
    /// 解決方法を求む。最初のシーンが読み込まれる前にこの関数を実行したい。
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += ((_, _) => FadeIn(3f, 0f));
    }
    /// <summary>
    /// フェードイン処理
    /// </summary>
    /// <param name="durationTime"> 明転に掛ける時間 </param>
    /// <param name="delayTime"> 遅延時間 </param>
    private static void FadeIn(float durationTime, float delayTime)
    {
        if (_dotween == null)
        {
            // Canvas用ゲームオブジェクトを作製
            // Canvasコンポーネントの割り当て,保存
            var canvasComponent = GetCanvas(new GameObject());

            // Image用ゲームオブジェクトを作製
            // Imageコンポーネントの割り当て,保存
            var imageComponent = GetImage(new GameObject(), canvasComponent, Color.black);

            // 演出を再生。完了時にCanvasオブジェクト･Imageオブジェクトを破棄,完了時処理を実行。
            _dotween = imageComponent.DOFade(0f, durationTime).SetDelay(delayTime).
                OnComplete(() =>
                {
                    GameObject.Destroy(canvasComponent.gameObject);
                    OnFadeInCompleted?.Invoke();
                    _dotween = null;
                });
        }
    }
    /// <summary>
    /// フェードアウトしシーンを変更する
    /// </summary>
    /// <param name="nextSceneName"> 次のシーンの名前 </param>
    /// <param name="durationTime"> 暗転に掛ける時間 </param>
    public static void FadeOut(string nextSceneName, float durationTime)
    {
        if (_dotween == null)
        {
            // Canvas用ゲームオブジェクトを作製
            // Canvasコンポーネントの割り当て,保存
            var canvasComponent = GetCanvas(new GameObject());
            // Image用ゲームオブジェクトを作製
            // Imageコンポーネントの割り当て,保存
            var imageComponent = GetImage(new GameObject(), canvasComponent, Color.clear);

            // 演出を再生。完了時にシーンを遷移。
            _dotween = imageComponent.DOFade(1f, durationTime).
                OnComplete(() =>
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
                    _dotween = null;
                });
        }
    }
    /// <summary>
    /// セットアップ済みのキャンバスを取得する
    /// </summary>
    /// <param name="owner"> 割り当てるゲームオブジェクト </param>
    /// <returns> セットアップ済みのキャンバスコンポーネント </returns>
    private static Canvas GetCanvas(GameObject owner)
    {
        // Canvasコンポーネントの割り当て･取得
        var result = owner.AddComponent<Canvas>();
        // RenderModeを指定する
        result.renderMode = RenderMode.ScreenSpaceOverlay;
        // 描画優先順序を設定
        result.sortingOrder = _sortOrder;

        return result;
    }
    /// <summary>
    /// セットアップ済みのイメージを取得する
    /// </summary>
    /// <param name="owner"> 割り当てるゲームオブジェクト </param>
    /// <param name="canvas"> キャンバスコンポーネント </param>
    /// <returns> セットアップ済みのイメージ </returns>
    private static Image GetImage(GameObject owner, Canvas canvas, Color color)
    {
        // Imageコンポーネントの割り当て･取得
        var result = owner.AddComponent<Image>();
        // 親オブジェクトを指定
        result.transform.SetParent(canvas.transform);
        // 位置を初期化(何もしなければ左下を中央とする座標になるため)
        result.rectTransform.localPosition = Vector3.zero;
        // アンカーポイントを設定する（ウィンドウの伸縮に対応する為の処理）
        result.rectTransform.anchorMin = new Vector2(0f, 0f);
        result.rectTransform.anchorMax = new Vector2(1f, 1f);
        // 色を設定
        result.color = color;
        return result;
    }
}
