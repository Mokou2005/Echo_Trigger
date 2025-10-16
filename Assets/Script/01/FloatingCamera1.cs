using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCamera1 : MonoBehaviour
{
    [Header("注視点:カメラが何処を見ているか")]
    public Transform m_GazingPoint;
    [Header("カメラが何処に移動しているか")]
    public Transform m_Target;
    [Header("カメラリンク")]
    public Transform m_Camera;

    public bool m_Flag;
    /// <summary>
    /// フレームではなく時間で起動するアップデート
    /// 重い処理とかに使用するが、場合によって処理をすっ飛ばす可能性がある
    /// </summary>
    private void FixedUpdate()
    {
        if (m_GazingPoint && m_Target)
        {

            m_Camera.LookAt(m_GazingPoint);

            //リープ使用フラグ
            if (m_Flag)
            {
                ///フローティングカメラの位置をフローティングカメラ台にゆっくり移動させる
                this.transform.position = Vector3.Slerp(
                    this.transform.position,
                    m_Target.position,
                    0.1f);
                ///フローティングカメラの向きをプレイヤーの位置情報から向き情報を獲得し、分割してゆっくり回転
                ///Slerpは、AからBまでの補完を行い、分割して値を提出する
                this.transform.rotation = Quaternion.Slerp(
                    this.transform.rotation,
                    m_Target.rotation,
                    0.1f);
            }
        }
    }
    private void LateUpdate()
    {
        if (m_GazingPoint && m_Target)
        {

            m_Camera.LookAt(m_GazingPoint);

            //リープ使用フラグ
            if (!m_Flag)
            {
                ///フローティングカメラの位置をターゲット固定
                this.transform.position = m_Target.position;
                ///フローティングカメラの向きをターゲット固定
                this.transform.rotation = m_Target.rotation;
            }

        }
    }
}
