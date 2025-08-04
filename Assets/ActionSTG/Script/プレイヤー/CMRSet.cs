using UnityEngine;

public class CMRSet : MonoBehaviour
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
}
