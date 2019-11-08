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
    GameObject animCamera = default;                // プレイヤー
    [SerializeField]
    PlayerHideController hideController = default;  // 隠れるアクション管理クラス
    [SerializeField]
    float sensitivity = 2;                    // カメラの感度

    /// <summary>
    /// 起動処理
    /// </summary>
    void OnEnable()
    {
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

        // 回転量を求める
        float Y_Rotation = Input.GetAxis("Mouse Y") * sensitivity;
        float X_Rotation = Input.GetAxis("Mouse X") * sensitivity;
        
        // ベッドに隠れている時の視点移動
        if (hideController.IsHideBed)
        {
            player.transform.Rotate(0, 0, -X_Rotation);
            transform.parent.Rotate(0, 0, -X_Rotation);
        }
        // ロッカーに隠れている時の視点移動
        else if (hideController.IsHideLocker)
        {
            player.transform.Rotate(0, X_Rotation, 0);
            transform.parent.Rotate(0, X_Rotation, 0);
        }
        // 通常時の視点移動
        else
        {
            player.transform.Rotate(0, X_Rotation, 0);
            transform.parent.Rotate(0, X_Rotation, 0);
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
}
