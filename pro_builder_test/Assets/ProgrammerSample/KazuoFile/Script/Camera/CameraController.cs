using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DirType = InteractFunction.DirType;

/// <summary>
/// カメラクラス
/// </summary>
/// NOTE:k.oishi アニメーションにカメラが含まれていると回転しなくなってしまったので、アニメーション用と移動用のカメラに分けました。
public class CameraController : MonoBehaviour
{
    /// <summary>
    /// 角度制限タイプ
    /// </summary>
    enum AngleLimitType
    {
        NORMAL,
        HIDE,
    }

    [SerializeField]
    Transform player = default;                         // プレイヤー
    [SerializeField]
    GameObject animCamera = default;                    // アニメーション用カメラ

    [SerializeField]
    PlayerHideController hideController = default;      // 隠れるアクション管理クラス
    [SerializeField]
    PlayerBreathController breathController = default;  // 隠れるアクション管理クラス
    [SerializeField]
    PlayerStateController stateController = default;    // ステート管理クラス

    [SerializeField]
    float sensitivity = 2;                              // カメラの感度

    /// <summary>
    /// 起動処理
    /// </summary>
    void OnEnable()
    {
        transform.parent.position = player.transform.position;
        transform.parent.rotation = player.rotation;
        transform.rotation = animCamera.transform.rotation;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // プレイヤーの頭の位置に合わせる
        transform.parent.position = player.transform.position;

        // 回転
        Rotation();
    }

    /// <summary>
    /// カメラの回転
    /// </summary>
    void Rotation()
    {
        // 回転量を求める
        float Y_Rotation = Input.GetAxis("Mouse Y") * sensitivity;
        float X_Rotation = Input.GetAxis("Mouse X") * sensitivity;

        // 動かしていない状態ならゆっくり視点を上下にサイン関数移動
        if (!GetDirectionKey() && Y_Rotation == 0 && X_Rotation == 0 && stateController.State == PlayerStateController.ActionStateType.WAIT)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x + WaitCameraSin(), transform.eulerAngles.y, transform.eulerAngles.z);
        }
        else
        {
            // ベッドに隠れている時の視点移動
            if (hideController.IsHideBed)
            {
                player.Rotate(0, 0, -X_Rotation);
                transform.parent.Rotate(0, 0, -X_Rotation);
            }
            // ロッカーに隠れている時の視点移動
            else if (hideController.IsHideLocker)
            {
                player.Rotate(0, X_Rotation, 0);
                transform.parent.Rotate(0, X_Rotation, 0);

                // 回転制限
                player.localEulerAngles = AngleLimit(AngleLimitType.HIDE);
                transform.parent.localEulerAngles = AngleLimit(AngleLimitType.HIDE);
            }
            // 通常時の視点移動
            else
            {
                player.Rotate(0, X_Rotation, 0);
                transform.parent.Rotate(0, X_Rotation, 0);
                transform.Rotate(-Y_Rotation, 0, 0);

                // 回転制限
                transform.localEulerAngles = AngleLimit(AngleLimitType.NORMAL);
            }
        }
    }

    /// <summary>
    /// カメラ回転を使うかどうか
    /// </summary>
    public void IsRotationCamera(bool isUse)
    {
       if(isUse)
       {
            // カメラ切り替え
            gameObject.SetActive(true);
            animCamera.SetActive(false);
       }
       else
       {
            // カメラ切り替え
            gameObject.SetActive(false);
            animCamera.SetActive(true);
       }
    }

    /// <summary>
    /// 隠れている時のカメラの回転制限
    /// </summary>
    Vector3 AngleLimit(AngleLimitType type)
    {
        // 回転値
        Vector3 angle = Vector3.zero;

        switch(type)
        {
            case AngleLimitType.NORMAL:
                // 基準値設定
                angle = transform.localEulerAngles;

                // 上限設定
                if (transform.localEulerAngles.x >= 30 && transform.localEulerAngles.x < 180)
                {
                    angle = new Vector3(30, transform.localEulerAngles.y, transform.localEulerAngles.z);
                }
                if (transform.localEulerAngles.x <= 330 && transform.localEulerAngles.x > 180)
                {
                    angle = new Vector3(330, transform.localEulerAngles.y, transform.localEulerAngles.z);
                }
                break;
            case AngleLimitType.HIDE:
                // 上限設定
                angle = HideAngleLimit();
                break;
        }
        return angle;
    }

    /// <summary>
    /// 隠れている時のカメラの回転制限
    /// </summary>
    Vector3 HideAngleLimit()
    {
        // 回転値
        Vector3 angle = transform.parent.localEulerAngles;

        switch (hideController.HideObjDir)
        {
            case DirType.FORWARD:
                // 上限設定
                if (transform.parent.localEulerAngles.y >= 200)
                {
                    angle = new Vector3(transform.parent.localEulerAngles.x, 200, transform.parent.localEulerAngles.z);
                }
                if (transform.parent.localEulerAngles.y <= 160)
                {
                    angle = new Vector3(transform.parent.localEulerAngles.x, 160, transform.parent.localEulerAngles.z);
                }
                break;
            case DirType.BACK:
                // 上限設定
                if (transform.parent.localEulerAngles.y >= 20 && transform.parent.localEulerAngles.y <= 180)
                {
                    angle = new Vector3(transform.parent.localEulerAngles.x, 20, transform.parent.localEulerAngles.z);
                }
                if (transform.parent.localEulerAngles.y <= 340 && transform.parent.localEulerAngles.y >= 180)
                {
                    angle = new Vector3(transform.parent.localEulerAngles.x, 340, transform.parent.localEulerAngles.z);
                }
                break;
            case DirType.RIGHT:
                // 上限設定
                if (transform.parent.localEulerAngles.y >= 290)
                {
                    angle = new Vector3(transform.parent.localEulerAngles.x, 290, transform.parent.localEulerAngles.z);
                }
                if (transform.parent.localEulerAngles.y <= 250)
                {
                    angle = new Vector3(transform.parent.localEulerAngles.x, 250, transform.parent.localEulerAngles.z);
                }
                break;
            case DirType.LEFT:
                // 上限設定
                if (transform.parent.localEulerAngles.y >= 110)
                {
                    angle = new Vector3(transform.parent.localEulerAngles.x, 110, transform.parent.localEulerAngles.z);
                }
                if (transform.parent.localEulerAngles.y <= 70)
                {
                    angle = new Vector3(transform.parent.localEulerAngles.x, 70, transform.parent.localEulerAngles.z);
                }
                break;
        }

        return angle;
    }

    /// <summary>
    /// 移動キーが入力されたかどうか
    /// </summary>
    /// <returns></returns>
    bool GetDirectionKey()
    {
        if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.D)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 待機時のカメラのサイン関数移動
    /// </summary>
    float WaitCameraSin()
    {
        float sin = 0;
        if (breathController.NowAmount < 100)
        {
            sin = Mathf.Sin(3 * Mathf.PI * Time.time) / 7.5f;
        }
        else
        {
            sin = Mathf.Sin(Time.time) / 50;
        }

        return sin;
    }
}
