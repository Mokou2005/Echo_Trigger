using UnityEngine;

public class Elevator : MonoBehaviour
{
    [Header("キャラクターアニメーター")]
    public Animator m_PushAnimator;
    [Header("ドアのアニメーター")]
    public Animator m_DoorAnimator;
    [Header("エレベータのアニメーター")]
    public Animator m_ElevatorAnimtor;
    [Header("UI表示用オブジェクト")]
    public GameObject m_Image;
    [Header("ドアの音")]
    public AudioClip m_DoorSound;
    private AudioSource m_Source;

    //プレイヤーが範囲以内にいるか？
    private bool m_IsArea=false;
    //エレベータは作動中か？
    private bool m_IsMoving = false;
    //現在の位置（false=下階、true=上階）
    private bool m_IsUp=false;
    //ドアの開閉状態
    private bool m_DoorOpen=false;
    private void Start()
    {
        m_Image.SetActive(false);
        m_Source = GetComponent<AudioSource>();
        // 必ず閉じた状態から開始
        m_DoorAnimator.SetBool("Open", false);
        m_DoorOpen=false ;
    }
    private void Update()
    {
        //プレイヤーがエリアに入っていて作動中じゃなければ（Eキーも押す）
        if (m_IsArea && !m_IsMoving && Input.GetKeyDown(KeyCode.E))
        {
            // 押したアニメーション再生
            m_PushAnimator.SetBool("Push", true);
            Invoke(nameof(ResetPush), 0.5f);

            if (!m_DoorOpen)
            {
                // ドアが閉じている → 開けるだけ
                m_DoorAnimator.SetBool("Open", true);
                m_Source.PlayOneShot(m_DoorSound);
                m_DoorOpen = true;

         
            }
            else
            {
                // ドアが開いている → 閉じてからエレベータを動かす
                m_DoorAnimator.SetBool("Open", false);
                m_Source.PlayOneShot(m_DoorSound);
                m_DoorOpen = false;
                // ドア閉まってから移動
                Invoke(nameof(StartElevator), 2.0f); 
            }


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
        m_IsMoving=true;
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
        //三秒後到着（ドア開く）
        Invoke(nameof(OpenDoor), 3.0f);
    }
    private void OpenDoor()
    {
        m_DoorAnimator.SetBool("Open", true);
        m_Source.PlayOneShot(m_DoorSound);
        m_DoorOpen = true;
        m_IsMoving = false;
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
