// 日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveLoadTester1 : MonoBehaviour
{
    private SaveLoad saveLoad = new SaveLoad();

    [SerializeField]
    private int saveID = default;

    [SerializeField]
    private SaveLoadTester2 _saveLoadTester2 = default;


    private void Update()
    {
        // Kキーでセーブ、Lキーでロードする
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("セーブ");
            saveLoad.Save(_saveLoadTester2, "save2", saveID);
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("ロード");
            _saveLoadTester2 = saveLoad.Load<SaveLoadTester2>("save2", saveID);
        }
    }
}
