using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DirType = InteractFunction.DirType;
using KeyType = KeyController.KeyType;
using StickType = KeyController.StickType;

/// <summary>
/// カメラクラス
/// </summary>
public class CameraController : MonoBehaviour
{
    /// <summary>
    /// 回転タイプ
    /// </summary>
    public enum RotationType
    {
        NORMAL,
        HIDEBED,
        HIDELOCKER,
        DEATH,
    }

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
    PlayerDamageController damageController = default;  // ダメージクラス
    [SerializeField]
    PlayerHideController hideController = default;      // 隠れるアクション管理クラス
    [SerializeField]
    KeyController keyController = default;              // キー操作クラス

    [SerializeField]
    float sensitivity = 2;                              // カメラの感度
    [SerializeField]
    float lookbackSensitivity = 20;                     // 振り返りの感度

    float lookBackAngle = 0;                            // 振り返りのアングル

    /// <summary>
    /// カメラの回転
    /// </summary>
    public void Rotation(RotationType type)
    {
        // 回転量を求める
        float Y_Rotation = keyController.GetStick(StickType.RIGHTSTICK).y * sensitivity;
        float X_Rotation = keyController.GetStick(StickType.RIGHTSTICK).x * sensitivity;

        switch (type)
        {
            case RotationType.NORMAL:
                player.Rotate(0, X_Rotation, 0);
                transform.Rotate(-Y_Rotation, 0, 0);

                // 振り返り
                if (keyController.GetKey(KeyType.LOOKBACK))
                {
                    LookBack();
                }
                else
                {
                    LookFront();
                }

                // 回転制限
                transform.localEulerAngles = NormalAngleLimit();
                break;
            case RotationType.HIDEBED:
                player.Rotate(0, 0, -X_Rotation);
                break;
            case RotationType.HIDELOCKER:
                player.Rotate(0, X_Rotation, 0);

                // 回転制限
                player.localEulerAngles = HideAngleLimit();
                break;
            case RotationType.DEATH:
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((damageController.EnemyPos.position - transform.position).normalized), Time.deltaTime);
                break;
        }
    }

    /// <summary>
    /// 通常時のカメラの回転制限
    /// </summary>
    Vector3 NormalAngleLimit()
    {
        // 回転値
        Vector3 angle = transform.localEulerAngles;

        // 上限設定
        if (transform.localEulerAngles.x >= 30 && transform.localEulerAngles.x < 180)
        {
            angle = new Vector3(30, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
        if (transform.localEulerAngles.x <= 330 && transform.localEulerAngles.x > 180)
        {
            angle = new Vector3(330, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }

        return angle;
    }

    /// <summary>
    /// 隠れている時のカメラの回転制限
    /// </summary>
    Vector3 HideAngleLimit()
    {
        // 回転値
        Vector3 angle = player.localEulerAngles;

        switch (hideController.HideObjDir)
        {
            case DirType.FORWARD:
                // 上限設定
                if (player.localEulerAngles.y >= 110)
                {
                    angle = new Vector3(player.localEulerAngles.x, 110, player.localEulerAngles.z);
                }
                if (player.localEulerAngles.y <= 70)
                {
                    angle = new Vector3(player.localEulerAngles.x, 70, player.localEulerAngles.z);
                }
                break;
            case DirType.BACK:
                // 上限設定
                if (player.localEulerAngles.y >= 290)
                {
                    angle = new Vector3(player.localEulerAngles.x, 290, player.localEulerAngles.z);
                }
                if (player.localEulerAngles.y <= 250)
                {
                    angle = new Vector3(player.localEulerAngles.x, 250, player.localEulerAngles.z);
                }

                break;
            case DirType.RIGHT:
                // 上限設定
                if (player.localEulerAngles.y >= 200)
                {
                    angle = new Vector3(player.localEulerAngles.x, 200, player.localEulerAngles.z);
                }
                if (player.localEulerAngles.y <= 160)
                {
                    angle = new Vector3(player.localEulerAngles.x, 160, player.localEulerAngles.z);
                }

                break;
            case DirType.LEFT:
                // 上限設定
                if (player.localEulerAngles.y >= 20 && player.localEulerAngles.y <= 180)
                {
                    angle = new Vector3(player.localEulerAngles.x, 20, player.localEulerAngles.z);
                }
                if (player.localEulerAngles.y <= 340 && player.localEulerAngles.y >= 180)
                {
                    angle = new Vector3(player.localEulerAngles.x, 340, player.localEulerAngles.z);
                }
                break;
        }

        return angle;
    }

    /// <summary>
    /// 振り返り
    /// </summary>
    void LookBack()
    {
        if (lookBackAngle < 180)
        {
            lookBackAngle += lookbackSensitivity;
            transform.Rotate(0, lookbackSensitivity, 0);
            if (lookBackAngle > 180)
            {
                lookBackAngle = 180;
            }
        }
    }

    /// <summary>
    /// 正面を向く
    /// </summary>
    void LookFront()
    {
        if (lookBackAngle > 0)
        {
            lookBackAngle -= lookbackSensitivity;
            transform.Rotate(0, -lookbackSensitivity, 0);
            if (lookBackAngle < 0)
            {
                lookBackAngle = 0;
            }
        }
    }
}
