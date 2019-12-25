using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParameterType = KageAnimParameterList.ParameterType;

/// <summary>
/// 影人間のステート：通常状態
/// </summary>
public class KageStateNormal : StateMachineBehaviour
{
    /// <summary>
    /// 通常状態時のステートの種類
    /// </summary>
    public enum StateKind
    {
        Standing = 1,   // 待機
        Loitering,      // 徘徊
    }

    // ステートの種類
    StateKind stateType = StateKind.Standing;

    // 影人間のステートのパラーメータを取得
    KageStateParameter stateParameter = null;

    // 影人間のパラメータークラス
    KageAnimParameterList animParameterList = null;

    // 聴こえる範囲のコライダーイベント
    ColliderEvent hearRangeCollider = null;

    // 影人間の戦闘範囲のコライダーイベント
    ColliderEvent fightingRangeCollider = null;

    // 影人間自身のコライダーのイベント
    ColliderEvent kageBodyCollider = null;

    // 視野の範囲
    KageFieldOfView fieldOfView = null;
    // 警戒範囲
    KageVigilanceRange vigilanceRange = null;

    // プレイヤーのハイドコントローラー
    PlayerHideController playerHideController = null;

    // サウンドエリアスポナー
    SoundAreaSpawner soundAreaSpawner = null;

    GameObject player = null;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // プレイヤー取得
        player = GameObject.FindGameObjectWithTag("Player") ?? player;

        // ステートパラメータを取得
        stateParameter = animator.GetComponent<KageStateParameter>() ?? stateParameter;
        // パラメータクラスを取得
        animParameterList = animator.GetComponent<KageAnimParameterList>() ?? animParameterList;
        // コライダークラスを取得
        hearRangeCollider = animator.transform.Find("Collider").Find("KageVigilanceRange").GetComponent<ColliderEvent>() ?? hearRangeCollider;
        // コライダークラス取得
        fightingRangeCollider = animator.transform.Find("Collider").Find("KageFightingRange").GetComponent<ColliderEvent>() ?? fightingRangeCollider;
        // コライダークラスを追加
        kageBodyCollider = animator.transform.Find("Collider").Find("KageAttackRange").GetComponent<ColliderEvent>() ?? kageBodyCollider;

        // 初期位置をリセット
        animator.transform.position = stateParameter.InitializePos;

        // ハイドコントローラーを取得
        playerHideController = FindObjectOfType<PlayerHideController>() ?? playerHideController;

        // コールバックをセットする
        hearRangeCollider.AddEnterListener(OnHearEnter);
        // コールバックをセットする
        fightingRangeCollider.AddEnterListener(OnPlayerDiscovery);
        // コールバックをセットする
        kageBodyCollider.AddEnterListener(OnPlayerDiscovery);

        // パラメータをセット
        stateType = stateParameter.StateNormalOfType;

        // 視野の範囲
        fieldOfView = animator.transform.Find("Collider").Find("KageFeildOfView").GetComponent<KageFieldOfView>() ?? fieldOfView;
        // 視野の範囲を設定する
        fieldOfView.ChangeDistance(KageState.Kind.Normal);
        fieldOfView.SetOnInViewRangeEvent(OnPlayerDiscovery);

        // 警戒範囲
        vigilanceRange = animator.transform.Find("Collider").Find("KageVigilanceRange").GetComponent<KageVigilanceRange>() ?? vigilanceRange;
        //警戒範囲の設定を行う
        vigilanceRange.ChangeRadius(KageState.Kind.Normal);

        // 指定された状態に変更
        animParameterList.SetInteger(ParameterType.normalBehaviourKindId, (int)stateType);

        soundAreaSpawner = FindObjectOfType<SoundAreaSpawner>() ?? soundAreaSpawner;
    }

    /// <summary>
    /// 物音が聞こえた瞬間のコールバック
    /// </summary>
    void OnHearEnter(Transform self,Collider target)
    {
        // プレイヤー自身はスキップ
        if (!animParameterList.GetBool(ParameterType.isVigilanceMode) && !animParameterList.GetBool(ParameterType.isFightingMode) && target.tag == "Player") { return; }

        if (playerHideController.IsHideBed || playerHideController.IsHideLocker)
        {
            // ハイドポイントに向かってレイを飛ばす
            Ray ray = new Ray(self.position, (playerHideController.HideObj.transform.position - self.position).normalized);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask(new string[] { "Bed","Locker", "Stage" })))
            {
                // レイがプレイヤー以外だったら
                if (hit.collider.gameObject.GetInstanceID() == playerHideController.HideObj.GetInstanceID())
                {
                    // 戦闘状態に移行
                    animParameterList.SetBool(ParameterType.isFightingMode, true);
                    return;
                }
            }
        }

        // 音のコライダーのサイズを取得
        animParameterList.SetFloat(ParameterType.heardSoundRadius, soundAreaSpawner.GetColliderRadius());

        // 警戒モードに変更
        animParameterList.SetBool(ParameterType.isVigilanceMode, true);
        // 接近対象の位置をセット
        animParameterList.SetFloat(ParameterType.targetPositionX, target.transform.position.x);
        animParameterList.SetFloat(ParameterType.targetPositionY, target.transform.position.y);
        animParameterList.SetFloat(ParameterType.targetPositionZ, target.transform.position.z);
        // 音が聞こえたときのトリガーをセット
        animParameterList.SetTrigger(ParameterType.perceiveSound);
    }

    /// <summary>
    /// プレイヤーを見つけた
    /// </summary>
    void OnPlayerDiscovery(Transform self, Collider target)
    {
        if (playerHideController.IsHideBed || playerHideController.IsHideLocker)
        {
            return;
        }

        if (target.tag == "PlayerNoise")
        {
            // プレイヤーに向かってレイを飛ばす
            Ray ray = new Ray(self.position, (player.transform.position - self.position).normalized);

            RaycastHit hit;
            if (Physics.Raycast(ray,out hit,Mathf.Infinity,LayerMask.GetMask(new string[] { "Player","Stage","Locker","Bed" })))
            {
                // レイがプレイヤー以外だったら
                if (hit.collider.tag != "Player")
                {
                    // 警戒状態に移行
                    animParameterList.SetBool(ParameterType.isVigilanceMode, true);
                    return;
                }
            }
        }

        // 警戒モードを解除
        animParameterList.SetBool(ParameterType.isVigilanceMode, false);
        // 戦闘モードに変更
        animParameterList.SetBool(ParameterType.isFightingMode, true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (animParameterList.GetInteger(ParameterType.normalBehaviourKindId) == 2)
        {
            return;
        }
    }
}
