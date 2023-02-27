// 日本語対応
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

/// <summary>
/// セーブ機能とロード機能を司るクラスの説明用のスクリプト
/// このクラスはAES暗号アルゴリズムを利用して暗号化,復号化を実現する。
/// 参考
/// 篠原さんのNotion : https://harvest-alto-8ef.notion.site/JSON-93f3e041b8bd4b649ca91819492bcffc
/// Wikipedia        : https://ja.wikipedia.org/wiki/Advanced_Encryption_Standard
/// 画像で理解する   : https://www.google.com/search?q=aes+%E6%9A%97%E5%8F%B7%E5%8C%96+%E3%82%A2%E3%83%AB%E3%82%B4%E3%83%AA%E3%82%BA%E3%83%A0&rlz=1C1TKQJ_jaJP1001JP1001&sxsrf=AJOqlzXAZmlXoHjdpZPYbRHL_JPgGgHoIA:1677416262876&source=lnms&tbm=isch&sa=X&ved=2ahUKEwjjvvTDnrP9AhVQ0GEKHSqjD2kQ_AUoAnoECAEQBA&biw=1442&bih=1376&dpr=1.5
/// </summary>

public class SaveLoadMyReference
{
    /// 鍵サイズとは、暗号化に使用する鍵の長さをビット単位で表したもの。
    /// 鍵サイズが大きいほど、暗号化の安全性が高くなりますが、処理速度が低下する可能性があります。
    /// 鍵サイズは、セキュリティに直接関係する重要な要素であり、鍵が長くなるほど、暗号化の解読が
    /// 困難になります。一方で、鍵サイズが小さい場合は、暗号化の解読が容易になる可能性があるため、
    /// 鍵サイズの適切な選択は、安全性を確保するために非常に重要です。
    /// 
    /// ブロックとは、情報を固定長の塊に区切るデータ処理の単位のこと。
    /// ブロックは、データを取り扱う上で、扱いやすくするために使用されます。
    /// 例えば、暗号化アルゴリズムでは、データを固定長のブロックに区切って、
    /// それぞれを独立に処理することが一般的です。
    /// ブロックには、固定長であるため、データの末尾には余分な空白が含まれることがあります。
    /// そのため、データの保存や伝送時には、パディング（余分なデータの追加）を行うことが
    /// 必要になることがあります。
    /// 
    /// IV : Initialization Vectorは、暗号化、復号化するときに使用する値。鍵と同様に秘匿される必要有り。
    /// IVがランダムな値であることで、攻撃者が暗号文から元の平文を復号することを困難にします。
    /// IVを使用しない場合、同じ鍵で暗号化されたメッセージが同じ暗号文になるため、
    /// 攻撃者は暗号文の出現頻度を分析することで、元の平文を復号することができる可能性があります。
    /// 
    /// 鍵は、暗号化、復号化するときに使用する値。
    /// 対称鍵暗号化では、暗号化されたデータを復号化するためには、暗号化に使用された鍵を知っている
    /// 必要があります。そのため、鍵を安全に管理することが重要であり、鍵が第三者に漏洩した場合は、
    /// 暗号化されたデータが簡単に解読される可能性があります。

    // AES設定値 aesIv・aesKeyは暗号化と複合化で同じものを使用する
    // ================================================= //
    private readonly int aesKeySize = 128;               // 鍵サイズ
    private readonly int aesBlockSize = 128;             // 一つのブロックのサイズ
    private readonly string aesIv = "6KGhH66PeU3cSLS7";  // 初期化ベクトル（Initialization Vectorの略称）
    private readonly string aesKey = "R38FYEzPyjxv0HrE"; // 鍵
    // ================================================= //


    /// <summary>
    /// 暗号化しオブジェクトをファイルに書きこむ機能
    /// </summary>
    /// <typeparam name="T">     セーブするオブジェクトの型 </typeparam>
    /// <param name="targetObj"> セーブするオブジェクト     </param>
    /// <param name="fileName">  セーブファイルの名前       </param>
    /// <param name="saveId">    セーブデータID             </param>
    public void Save<T>(T targetObj, string fileName, int saveId)
     where T : /* struct, class */ new() // MonoBehaviourを継承しておらず、[System.Serializable]属性を適応しているクラスか構造体に制限したい。
    // https://docs.unity3d.com/ja/2020.3/ScriptReference/JsonUtility.FromJson.html
    {
        // ファイルへのパスを取得
        var path = Application.persistentDataPath + fileName + saveId + ".json";
        // 保存するデータを作成
        var saveData = JsonUtility.ToJson(targetObj);
        // ジェイソンをUTF8形式の文字列をエンコード（変換）し、そのバイト列を取得する。
        var jsonByte = Encoding.UTF8.GetBytes(saveData);
        // 指定のバイト列を暗号化したものを取得する。
        jsonByte = AesEncrypt(jsonByte);

        // データをファイルに書き込む
        File.WriteAllBytes(path, jsonByte);
    }

    /// <summary>
    /// 復号化しファイルから情報を取得する機能
    /// </summary>
    /// <typeparam name="T">    ロードするオブジェクトの型 </typeparam>
    /// <param name="fileName"> ロードファイルの名前       </param>
    /// <param name="saveId">   セーブデータID             </param>
    /// <returns> ロードしたオブジェクト。ロードに失敗した場合nullを返す。 </returns>
    public T Load<T>(string fileName, int saveId)
        where T : class
    {
        // ファイルへのパスを取得する
        var path = Application.persistentDataPath + fileName + saveId + ".json";
        // ファイルを読み込む
        var byteData = File.ReadAllBytes(path);
        // 復号化する
        byteData = AesDecrypt(byteData);
        // バイト列をstring型にエンコード（変換）する
        var json = Encoding.UTF8.GetString(byteData);
        // json形式の文字列をT型に戻し、結果を返す。
        return JsonUtility.FromJson<T>(json);
    }

    /// <summary> AES暗号化 </summary>
    /// <param name="byteText"> 暗号化するテキスト(バイト列) </param>
    /// <returns> 暗号化した結果のバイト列 </returns>
    private byte[] AesEncrypt(byte[] byteText)
    {
        // AESマネージャー取得
        var aes = GetAesManager(aesKeySize, aesBlockSize, aesIv, aesKey);
        // 暗号化処理を実行し、結果を返す。
        //     結果        = AESマネージャー.対称暗号化オブジェクト.暗号化処理(暗号化対象,0,長さ)
        byte[] encryptText = aes.CreateEncryptor().TransformFinalBlock(byteText, 0, byteText.Length);

        return encryptText;
    }

    /// <summary> AES復号化 </summary>
    /// <param name="byteText"> 復号化するテキスト(バイト列) </param>
    /// <returns> 復号化した結果のバイト列 </returns>
    private byte[] AesDecrypt(byte[] byteText)
    {
        // AESマネージャー取得
        var aes = GetAesManager(aesKeySize, aesBlockSize, aesIv, aesKey);
        // 復号化処理を実行し、結果を返す。
        //     結果        = AESマネージャー.対称復号化オブジェクト.復号化処理(復号化対象,0,長さ)
        byte[] decryptText = aes.CreateDecryptor().TransformFinalBlock(byteText, 0, byteText.Length);

        return decryptText;
    }

    /// <summary> セットアップ済みのAesManaged型のオブジェクトを取得 </summary>
    /// <param name="keySize">   暗号化鍵の長さ                                      </param>
    /// <param name="blockSize"> ブロックサイズ                                      </param>
    /// <param name="iv">        初期化ベクトル(半角x文字（8bit * x = [keySize]bit)) </param>
    /// <param name="key">       暗号化鍵　　　(半角x文字（8bit * x = [keySize]bit)) </param>
    ///                                             ↑このあたりがよくわからない
    private AesManaged GetAesManager(int keySize, int blockSize, string iv, string key)
    {
        AesManaged aes = new AesManaged();
        aes.KeySize = keySize;                 // 鍵サイズを指定する。
        aes.BlockSize = blockSize;             // 一つのブロックのサイズを指定する。
        aes.IV = Encoding.UTF8.GetBytes(iv);   // 初期化ベクトルを指定する。
        aes.Key = Encoding.UTF8.GetBytes(key); // 鍵を指定する。

        // 暗号化モードを指定する。CipherMode 列挙型とは :https://learn.microsoft.com/ja-jp/dotnet/api/system.security.cryptography.ciphermode?view=net-7.0
        aes.Mode = CipherMode.CBC;
        // パディングモードを指定する。 PaddingMode列挙型とは : https://learn.microsoft.com/ja-jp/dotnet/api/system.security.cryptography.paddingmode?view=net-7.0
        aes.Padding = PaddingMode.PKCS7;
        return aes;
    }
}
