using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DirType = InteractFunction.DirType;

/// <summary>
/// カメラクラス
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform player = default;                     // プレイヤー
    [SerializeField]
    PlayerHideController hideController = default;  // 隠れるアクション管理クラス
    [SerializeField]
    float sensitivity = default;                    // カメラの感度

    [SerializeField]
    float bedRotationLimit = 50;
    [SerializeField]
    float lockerRotationLimit = 20;
    [SerializeField]
    bool isShake = true;

    /// <summary>
    /// 起動処理
    /// </summary>
    void OnEnable()
    {
        // 起動時はプレイヤーの正面を向く
        transform.Rotate(0, 0, 0);
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // 回転量を求める
        float Y_Rotation = Input.GetAxis("Mouse Y") * sensitivity;
        float X_Rotation = Input.GetAxis("Mouse X") * sensitivity;

        // ベッドに隠れている時の視点移動
        if (hideController.IsHideBed)
        {
            player.transform.Rotate(0, 0, -X_Rotation);
        }
        // ロッカーに隠れている時の視点移動
        else if (hideController.IsHideLocker)
        {
            player.transform.Rotate(0, X_Rotation, 0);
        }
        // 通常時の視点移動
        else
        {
            player.transform.Rotate(0, X_Rotation, 0);
            transform.Rotate(-Y_Rotation, 0, 0);

            // 上限設定
            if (transform.localEulerAngles.x >= 30 && transform.localEulerAngles.x < 180)
            {
                transform.localEulerAngles = new Vector3(30, transform.localEulerAngles.y, transform.localEulerAngles.z);
            }
            if (transform.localEulerAngles.x <= 330 && transform.localEulerAngles.x > 180)
            {
                transform.localEulerAngles = new Vector3(330, transform.localEulerAngles.y, transform.localEulerAngles.z);
            }
        }
    }

    /// <summary>
    /// 移動時のカメラの揺れ
    /// </summary>
    /// <param name="power">揺れの強さ</param>
    public void MoveShake(float power)
    {
        if (isShake)
        {
            transform.position = new Vector3(transform.position.x, 1.65f + Mathf.PingPong(Time.time, power), transform.position.z);
        }
    }
}
