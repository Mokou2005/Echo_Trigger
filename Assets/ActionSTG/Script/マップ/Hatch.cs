using UnityEngine;
/// <summary>
/// ハッチの処理
/// </summary>
public class Hatch : MonoBehaviour
{
    [Header("テレポートする場所"),SerializeField]
    private Transform m_TPPosition;

    /// <summary>
    /// エリアに入ったらテレポートの処理へ
    /// </summary>
    /// <param name="other">プレイヤー</param>
    private void OnTriggerEnter(Collider other)
    {
        //プレイヤーならテレポートの処理へ
        if (other.gameObject.CompareTag("Player"))
        {
            HatchTP();
        }
    }

    /// <summary>
    /// テレポートの処理
    /// </summary>
    private void HatchTP()
    {

    }
}
