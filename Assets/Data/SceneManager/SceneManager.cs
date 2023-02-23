using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SceneManager
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
    /// 起動時にFadeInを登録する。<br/>
    /// このクラスがプロジェクトに存在するだけで強制的に追加されるので <br/>
    /// 配布、複製する際は注意すること。<br/>
    /// Editor上で実行する際,Enter Play Mode Optionを使用すると予期せぬ挙動となる。<br/>
    /// 解決方法を求む。最初のシーンが読み込まれる前にこの関数を実行したい。
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += ((_, _) => FadeIn(5f, 0.6f));
    }
    /// <summary>
    /// フェードイン処理
    /// </summary>
    /// <param name="durationTime"> 明転に掛ける時間 </param>
    /// <param name="delayTime"> 遅延時間 </param>
    private static void FadeIn(float durationTime, float delayTime)
    {
        // Canvas用ゲームオブジェクトを作製
        // Canvasコンポーネントの割り当て,保存
        var canvasComponent = GetCanvas(new GameObject());

        // Image用ゲームオブジェクトを作製
        // Imageコンポーネントの割り当て,保存
        var imageComponent = GetImage(new GameObject(), canvasComponent);

        // 演出を再生。完了時にCanvasオブジェクト･Imageオブジェクトを破棄,完了時処理を実行。
        imageComponent.DOFade(0f, durationTime).SetDelay(delayTime).
            OnComplete(() =>
            {
                GameObject.Destroy(canvasComponent.gameObject);
                OnFadeInCompleted?.Invoke();
            });
    }
    /// <summary>
    /// フェードアウトしシーンを変更する
    /// </summary>
    /// <param name="nextSceneName"> 次のシーンの名前 </param>
    /// <param name="durationTime"> 暗転に掛ける時間 </param>
    public static void FadeOut(string nextSceneName, float durationTime)
    {
        // Canvas用ゲームオブジェクトを作製
        // Canvasコンポーネントの割り当て,保存
        var canvasComponent = GetCanvas(new GameObject());

        // Image用ゲームオブジェクトを作製
        // Imageコンポーネントの割り当て,保存
        var imageComponent = GetImage(new GameObject(), canvasComponent);

        // 演出を再生。完了時にシーンを遷移。
        imageComponent.DOFade(1f, durationTime).
            OnComplete(() =>
                UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName));
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
    /// <returns></returns>
    private static Image GetImage(GameObject owner, Canvas canvas)
    {
        // Imageコンポーネントの割り当て･取得
        var result = owner.AddComponent<Image>();
        // 親オブジェクトを指定
        result.transform.SetParent(canvas.transform);
        // 位置を初期化(何もしなければ左下を中央とする座標になるため)
        result.rectTransform.localPosition = Vector3.zero;
        // 幅と高さを設定
        var size = result.rectTransform.sizeDelta;
        size.x = Screen.width;
        size.y = Screen.height;
        result.rectTransform.sizeDelta = size;
        // 色を設定
        result.color = Color.black;

        return result;
    }
}
