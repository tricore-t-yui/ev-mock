using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoveType = PlayerStateController.ActionStateType;

/// <summary>
/// パラメーター表示クラス
/// </summary>
public class ParameterText : MonoBehaviour
{
    [SerializeField]
    bool isDisplay = false;     // テキストを表示するかどうか

    [SerializeField]
    PlayerStateController stateController = default;        // 状態管理クラス
    [SerializeField]
    playerStaminaController staminaController = default;    // プレイヤーのスタミナクラス
    [SerializeField]
    PlayerHealthController healthController = default;      // プレイヤーの体力クラス
    [SerializeField]
    PlayerBreathController breathController = default;      // プレイヤーの息クラス
    [SerializeField]
    PlayerHideController hideController = default;          // プレイヤーの隠れるアクションクラス
    [SerializeField]
    SoundAreaSpawner soundAreaSpawner = default;            // 音領域生成クラス

    [SerializeField]
    Text[] texts = default;     // テキスト

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        foreach(var item in texts)
        {
            if (isDisplay)
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        if(isDisplay)
        {
            texts[0].text = "今のプレイヤーの行動 : " + StateName();
            texts[1].text = "ＨＰ　　 : " + healthController.NowAmount.ToString();
            texts[2].text = "息　　　 : " + breathController.NowAmount.ToString();
            texts[3].text = "スタミナ : " + staminaController.NowAmount.ToString();
            texts[4].text = "音レベル : " + soundAreaSpawner.TotalSoundLevel.ToString();
            if (stateController.IsInvincible)
            {
                texts[5].text = "無敵モードON : Lキーで変更";
            }
            else
            {
                texts[5].text = "無敵モードOFF : Lキーで変更";
            }
        }
    }

    /// <summary>
    /// ステートの名前
    /// </summary>
    string StateName()
    {
        string stateName = "待機";

        switch (stateController.State)
        {
            case MoveType.WAIT: stateName = "待機"; break;
            case MoveType.WALK: stateName = "歩き"; break;
            case MoveType.DASH: stateName = "ダッシュ"; break;
            case MoveType.BREATHHOLD: stateName = "息止め"; break;
            case MoveType.BREATHHOLDMOVE: stateName = "息止め歩き"; break;
            case MoveType.DOOROPEN: stateName = "ドア開閉"; break;
            case MoveType.HIDE:
                if (hideController.IsHideStealth())
                {
                    stateName = "隠れて息止め"; break;
                }
                else
                {
                    stateName = "隠れる"; break;
                }
            case MoveType.DEEPBREATH: stateName = "深呼吸"; break;
            case MoveType.BREATHLESSNESS: stateName = "息切れ"; break;
            case MoveType.DAMAGE: stateName = "ダメージ"; break;
            case MoveType.DOLLGET: stateName = "人形ゲット"; break;
            case MoveType.TRAP: stateName = "罠"; break;
            default: stateName = "ステート外";break;
        }

        if(!stateController.IsShoes)
        {
            stateName = "裸足" + stateName;
        }
        if (stateController.IsSquat)
        {
            stateName = "しゃがみ" + stateName;
        }

        return stateName;
    }
}
