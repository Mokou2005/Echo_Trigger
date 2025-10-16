using UnityEngine;


/*public class CMRSet : MonoBehaviour
{
    public Transform m_CMRBase;

    void LateUpdate()
    {
        if (m_CMRBase)
        {
            transform.position = m_CMRBase.position;
            transform.rotation = m_CMRBase.rotation;
        
        }
    }
}*/


using UnityEngine;

public class CMRSet : MonoBehaviour
{
    [Header("カメラの基準となるオブジェクト（プレイヤーなど）")]
    public Transform m_CMRBase;

   
    [Header("回転設定")]
    public float pitchSpeed = 60f;   // 視線の上下スピード
    public float minPitch = -30f;    // 下を向く限界角度
    public float maxPitch = 45f;     // 上を向く限界角度

    private float m_HeightOffset = 0f; // 現在の上下オフセット値
    private float m_Pitch = 0f;        // 現在の上下回転角度

    void LateUpdate()
    {
        if (m_CMRBase)
        {
            // 🎯 マウス入力で上下移動
            float mouseY = Input.GetAxis("Mouse Y");
 

            // 🎯 同時にカメラの向きも調整
            m_Pitch -= mouseY * pitchSpeed * Time.deltaTime;
            m_Pitch = Mathf.Clamp(m_Pitch, minPitch, maxPitch);

            // 🎯 新しい位置（ベースの位置 + 高さオフセット）
            Vector3 newPos = m_CMRBase.position + new Vector3(0, m_HeightOffset, 0);
            transform.position = newPos;

            // 🎯 ベースの回転を継承しつつ、上下角度を加える
            Quaternion baseRot = m_CMRBase.rotation;
            Quaternion pitchRot = Quaternion.Euler(m_Pitch, 0f, 0f);
            transform.rotation = baseRot * pitchRot;
        }
    }
}



