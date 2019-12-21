﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// インタラクト関数クラス
/// </summary>
public class InteractFunction : MonoBehaviour
{
    /// <summary>
    /// 各オブジェクトの向きタイプ
    /// </summary>
    public enum DirType
    {
        FORWARD,
        BACK,
        LEFT,
        RIGHT,
    }

    /// <summary>
    /// オブジェクトの向きに合わせた位置合わせ
    /// </summary>
    /// <param name="type">オブジェクトのタイプ</param>
    public Vector3 InitPosition(DirType type, Transform player, Transform target)
    {
        Vector3 position = Vector3.zero;

        switch (type)
        {
            case DirType.FORWARD:
                position = new Vector3(target.transform.position.x, player.position.y, target.transform.position.z - target.localScale.z);
                break;
            case DirType.BACK:
                position = new Vector3(target.transform.position.x, player.position.y, target.transform.position.z + target.localScale.z);
                break;
            case DirType.RIGHT:
                position = new Vector3(target.transform.position.x - target.localScale.x, player.position.y, target.transform.position.z);
                break;
            case DirType.LEFT:
                position = new Vector3(target.transform.position.x + target.localScale.x, player.position.y, target.transform.position.z);
                break;
        }

        return position;
    }

    /// <summary>
    /// オブジェクトの向きに合わせた角度合わせ
    /// </summary>
    /// <param name="type">オブジェクトのタイプ</param>
    public Quaternion InitRotation(DirType type)
    {
        Quaternion rotation = default;

        switch (type)
        {
            case DirType.FORWARD:
                rotation = Quaternion.Euler(0, 0, 0);
                break;
            case DirType.BACK:
                rotation = Quaternion.Euler(0, 180, 0);
                break;
            case DirType.RIGHT:
                rotation = Quaternion.Euler(0, 90, 0);
                break;
            case DirType.LEFT:
                rotation = Quaternion.Euler(0, 270, 0);
                break;
        }

        return rotation;
    }
}