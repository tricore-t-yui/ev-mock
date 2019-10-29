using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnEnterDelegate(int areaNameHash, int charaNameHash);
public delegate void OnExitDelegate(int areaNameHash, int charaNameHash);

/// <summary>
/// エリアマネージャー（仮）
/// </summary>
public class AreaManager : MonoBehaviour
{
    // エリアのリスト
    [SerializeField]
    List<string> areaNameList = default;

    // 各エリアのキャラクター
    Dictionary<int, List<int>> areaExistCharacters = new Dictionary<int, List<int>>();
    
    /// <summary>
    /// 開始
    /// </summary>
    void Start()
    {
        foreach(string areaName in areaNameList)
        {
            // エリア名からハッシュ値を取得
            // note : オブジェクト名がそのままエリア名になる
            int areaHash = areaName.GetHashCode();
            // ハッシュ値をキーとしてDictionaryに追加
            areaExistCharacters.Add(areaHash, new List<int>());
        }

        // 各コールバック関数を渡す
        foreach(Transform gate in transform)
        {
            foreach(AreaEnterWatcher areaEnterWatcher in GetComponentsInChildren<AreaEnterWatcher>())
            {
                areaEnterWatcher.SetOnEnterDelegate(OnEnter);
                areaEnterWatcher.SetOnExitDelegate(OnExit);
            }
        }
    }

    /// <summary>
    /// エリアに入った時のコールバック
    /// </summary>
    /// <param name="areaName">入ったエリア名のハッシュ</param>
    /// <param name="characterName">入ったキャラクター名のハッシュ</param>
    public void OnEnter(int areaNameHash, int charaNameHash)
    {
        // エリアIDの要素にキャラクターを追加
        areaExistCharacters[areaNameHash].Add(charaNameHash);
    }

    /// <summary>
    /// エリアから出たときのコールバック
    /// </summary>
    /// <param name="areaName">入ったエリア名のハッシュ</param>
    /// <param name="characterName">入ったキャラクター名のハッシュ</param>
    public void OnExit(int areaNameHash, int charaNameHash)
    {
        // エリアIDの要素からキャラクターを削除
        areaExistCharacters[areaNameHash].Remove(charaNameHash);
    }
}
