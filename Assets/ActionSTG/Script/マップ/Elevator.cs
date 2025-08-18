using UnityEngine;

public class Elevator : MonoBehaviour
{
    [Header("キャラクターアニメーター")]
    public Animator m_PushAnimator;
    [Header("エレベータのアニメーター")]
    public Animator m_ElevatorAnimtor;
    [Header("UI表示用オブジェクト")]
    public GameObject m_Image;
    [Header("Doorのscript")]
    public DoorPush m_Door;

    //プレイヤーが範囲以内にいるか？
    private bool m_IsArea = false;
    //エレベータは作動中か？
    private bool m_IsMoving = false;
    //現在の位置（false=下階、true=上階）
    private bool m_IsUp = false;
    //ドアの開閉状態
    private bool m_DoorOpen = false;
    private void Start()
    {
        m_Image.SetActive(false);
        m_DoorOpen = false;
    }
    private void Update()
    {
        //プレイヤーがエリアに入っていて作動中じゃなければ（Eキーも押す）
        if (m_IsArea && !m_IsMoving && Input.GetKeyDown(KeyCode.E))
        {
            // 押したアニメーション再生
            m_PushAnimator.SetBool("Push", true);
            Invoke(nameof(ResetPush), 0.5f);
            Invoke(nameof(StartElevator), 2.0f);
            m_Image.SetActive(false);
        }
    }
    //アニメーターリセット
    private void ResetPush()
    {
        m_PushAnimator.SetBool("Push", false);
    }

    private void StartElevator()
    {
        //作動！
        m_IsMoving = true;
        if (!m_IsUp)
        {
            // 下階 → 上階へ
            m_ElevatorAnimtor.SetTrigger("MoveUp");
            m_IsUp = true;
        }
        else
        {
            // 上階 → 下階へ
            m_ElevatorAnimtor.SetTrigger("MoveDown");
            m_IsUp = false;
        }
        //アニメーション３秒後解除
        Invoke(nameof(StopElevator), 5.0f);
    }
    private void StopElevator()
    {
        m_IsMoving = false;
        //到着したらドア開く
        if (m_Door != null)
        {
            m_Door.ForceDoor(true);
            // 10秒後に閉じる
            Invoke(nameof(CloseDoor), 10f);
        }
    }
    //閉まる（自動ドア）
    private void CloseDoor()
    {
        if (m_Door != null)
        {
            m_Door.ForceDoor(false);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Parameta>())
        {
            m_IsArea = true;
            if (!m_IsMoving) m_Image.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Parameta>())
        {
            m_IsArea = false;
            m_Image.SetActive(false);
        }
    }

}
