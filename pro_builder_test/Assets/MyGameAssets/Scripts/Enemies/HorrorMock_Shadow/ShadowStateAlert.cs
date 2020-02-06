using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowStateAlert : MonoBehaviour
{
    [SerializeField]
    Animator animator = default;

    // 影人間のメッシュ
    [SerializeField]
    SkinnedMeshRenderer shadowBodyMesh = default;

    [Space(15)]

    [SerializeField]
    float safeSoundLevel = 5;

    [SerializeField]
    float alertTimeOfSecond = 1;
    float alertTimeCounter = 0;

    // サウンドエリアスポナー
    SoundAreaSpawner soundAreaSpawner = null;

    // プレイヤーのサウンドレベル
    float currentSoundLevel = 0;

    void Start()
    {
        // サウンドエリアスポナーを取得
        soundAreaSpawner = FindObjectOfType<SoundAreaSpawner>();
        if (soundAreaSpawner == null) { Debug.LogError("soundAreaSpawner is null"); }
    }

    void OnEnable()
    {
        // 影人間が姿を現す
        shadowBodyMesh.enabled = true;

        // 猶予時間をセット
        alertTimeCounter = alertTimeOfSecond;
    }

    void Update()
    {
        // 時間を減らしていく
        alertTimeCounter -= Time.deltaTime;

        // 現在のサウンドレベルを取得
        currentSoundLevel = soundAreaSpawner.TotalSoundLevel;
        // ０以下は０にする
        if (currentSoundLevel <= 0) { currentSoundLevel = 0; }

        if (currentSoundLevel > safeSoundLevel)
        {
            // 警戒に移行
            OnStateCoution();
        }

        // サウンドレベルを丸め込む
        alertTimeCounter -= currentSoundLevel * 0.1f;

        // 猶予時間が０になったら
        if (alertTimeCounter <= 0)
        {
            // 警戒に移行
            OnStateCoution();
        }
    }

    void OnDisable()
    {

    }

    public void OnStateCoution()
    {
        // 警戒に移行
        animator.SetInteger("StateTypeId", (int)ShadowParameter.StateType.Caution);
        // 待機フラグをオフにする
        animator.SetBool("IsAlertWaiting", false);
    }
}
