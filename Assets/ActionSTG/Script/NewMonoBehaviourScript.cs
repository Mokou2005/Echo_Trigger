using UnityEngine;
using UnityEngine.Animations;
using Cinemachine;


public class NewMonoBehaviourScript : MonoBehaviour
{
    public Transform m_eye;
    public AxisState m_Vertical;
    public AxisState m_Horizontal;

    private void Update()
    {
        // ì¸óÕÇAxisStateÇ…ìKópÇ∑ÇÈ
        m_Horizontal.m_InputAxisValue = Input.GetAxis("Mouse X");
        m_Vertical.m_InputAxisValue = Input.GetAxis("Mouse Y");
        var leftStick = new Vector3(m_Horizontal.m_InputAxisValue, 0, m_Vertical.m_InputAxisValue).normalized;
        m_Horizontal.Update(Time.deltaTime);
        m_Vertical.Update(Time.deltaTime);
        //å¸Ç´í≤êÆ
        var horizontalRotation = Quaternion.AngleAxis(m_Horizontal.Value, Vector3.up);
        var verticalRotation = Quaternion.AngleAxis(m_Vertical.Value, Vector3.right);
        transform.rotation=horizontalRotation;
        m_eye.localRotation = verticalRotation;
    }
}
