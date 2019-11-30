using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public delegate void OnEnterDelegate(string areaName, string charaName);
public delegate void OnExitDelegate(string areaName, string charaName);

/// <summary>
/// エリアマネージャー（仮）
/// </summary>
public class AreaManager : MonoBehaviour
{
    // エリアのリスト
    [SerializeField]
    List<string> areaNameList = default;

    // 各エリアのキャラクター
    Dictionary<string, List<string>> areaExistCharacters = new Dictionary<string, List<string>>();

    /// <summary>
    /// 開始
    /// </summary>
    void Start()
    {
        foreach(string areaName in areaNameList)
        {
            // ハッシュ値をキーとしてDictionaryに追加
            areaExistCharacters.Add(areaName, new List<string>());
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
    public void OnEnter(string areaName, string charaName)
    {
        // エリアIDの要素にキャラクターを追加
        areaExistCharacters[areaName].Add(charaName);
    }

    /// <summary>
    /// エリアから出たときのコールバック
    /// </summary>
    public void OnExit(string areaName, string charaName)
    {
        // エリアIDの要素からキャラクターを削除
        areaExistCharacters[areaName].RemoveAll(x => x == charaName) ;
    }

    /// <summary>
    /// 指定したキャラクターがどのエリアにいるか調べる
    /// </summary>
    public string GetExistAreaToCharacter(string charaNameHash)
    {
        foreach(string areaName in areaExistCharacters.Keys)
        {
            // 指定のキャラクターがいるか検索
            if (areaExistCharacters[areaName].Find(x => x == charaNameHash) != default)
            {
                // エリアを返す
                return areaName;
            }
        }

        // 見つからなかったらintの規定値を返す
        Debug.Log("character not found");
        return default;
    }
}
