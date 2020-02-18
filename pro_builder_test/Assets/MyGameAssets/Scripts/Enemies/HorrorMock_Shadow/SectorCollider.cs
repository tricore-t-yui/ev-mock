using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public class SectorCollider : MonoBehaviour
{
    // コライダー用のUnityEvent
    [System.Serializable]
    public class ColliderEvent : UnityEvent<Collider> { }
    // スフィアコライダー
    SphereCollider sphereCollider = default;

    [SerializeField]
    Transform baseTransform = default;
    [SerializeField]
    Quaternion rotationOffset = Quaternion.identity;
    [SerializeField]
    LayerMask raycastMask = default;
    [SerializeField]
    string targetLayerName = null;
    int targetLayer = 0;

    [SerializeField]
    float angle = 0;
    public float Angle { get { return angle; } set { angle = value; } }

    [SerializeField]
    float distance = 0;
    public float Distance { get { return distance; } set { distance = value; } }

    [SerializeField]
    Color borderColor = default;

    [SerializeField]
    ColliderEvent visibleEnter = default;
    [SerializeField]
    ColliderEvent visibleStay = default;
    [SerializeField]
    ColliderEvent visibleExit = default;

    void Start()
    {
        GameObject colliderObject = new GameObject("Collider");
        colliderObject.transform.parent = transform;
        colliderObject.transform.localPosition = Vector3.zero;
        Rigidbody rb = transform.gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        sphereCollider = colliderObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = distance;

        targetLayer = LayerMask.NameToLayer(targetLayerName);
    }

    private void Update()
    {
        sphereCollider.radius = distance;
        transform.rotation = baseTransform.rotation * rotationOffset;
    }

    bool IsVisibleObject(Collider target)
    {
        Ray ray = new Ray(transform.position, (target.transform.position - transform.position).normalized);

        // 当たらなければ終了
        RaycastHit raycastHit;
        if (!Physics.Raycast(ray,out raycastHit,distance,raycastMask)) { return false; }

        // 対象のオブジェクトの当たっている
        if (raycastHit.collider.gameObject.layer == targetLayer)
        {
            float dot = Vector3.Angle(transform.forward, ray.direction);

            // 扇形の範囲内に入っている
            if (dot < (angle * 0.5f) && raycastHit.distance < distance)
            {
                // デバッグ用の線を描画
                Debug.DrawLine(transform.position, raycastHit.point,Color.yellow);
                return true;
            }
        }
        return false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsVisibleObject(other))
        {
            visibleEnter.Invoke(other);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (IsVisibleObject(other))
        {
            visibleStay.Invoke(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (IsVisibleObject(other))
        {
            visibleExit.Invoke(other);
        }
    }

    void OnDrawGizmos()
    {
        // 左側の境界ベクトル
        Vector3 leftBorder = (Quaternion.AngleAxis(angle * 0.5f, transform.right * -1) * transform.forward) * distance;
        // 右側の境界ベクトル
        Vector3 rightBorder = (Quaternion.AngleAxis(angle * 0.5f, transform.right) * transform.forward) * distance;
        // 上側の境界ベクトル
        Vector3 upBorder = (Quaternion.AngleAxis(angle * 0.5f, transform.up) * transform.forward) * distance;
        // 下側の境界ベクトル
        Vector3 downBorder = (Quaternion.AngleAxis(angle * 0.5f, transform.up * -1) * transform.forward) * distance;
        
        // デバッグ用に境界ベクトルを表示
        Debug.DrawRay(transform.position, leftBorder,borderColor);
        Debug.DrawRay(transform.position, rightBorder, borderColor);
        Debug.DrawRay(transform.position, upBorder, borderColor);
        Debug.DrawRay(transform.position, downBorder, borderColor);
    }
}