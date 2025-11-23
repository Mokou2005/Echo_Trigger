using UnityEngine;

public class Throw : MonoBehaviour
{
    [Header("自動アニメーター")]
    [SerializeField] private Animator m_animator;
    //構えた状態かどうか
    [SerializeField] private bool m_IsBrace = false;
    //投げたフラグ
    [SerializeField] public bool m_IsThrow = false;


    private void Start()
    {
        if (m_animator == null)
        {
            m_animator = GetComponent<Animator>();
            if (m_animator == null)
            {
                Debug.LogError("アニメーターが入ってません。");
            }
        }
    }

    private void Update()
    {
        // Qボタンは構え
        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_IsBrace=!m_IsBrace;   
            if (m_IsBrace)
            {
                Debug.Log("構えた");
                m_animator.SetBool("Brace", m_IsBrace);
                m_IsThrow = true;   
            }
            else
            {
                Debug.Log("しまった");
                m_animator.SetBool("Brace", m_IsBrace);
                m_IsThrow = false;
            }

        }

        // 左クリックで投げる
        if (Input.GetMouseButtonDown(0)&&m_IsThrow)
        {
            Debug.Log("投げた");
            m_IsBrace = false;

            
            // Throw アニメーション再生
            m_animator.SetTrigger("Throw");

            

           
        }
    }
    //アニメーションイベントで制御
    void ThrowEnd()
    {
        m_IsThrow = false;
        m_animator.SetBool("Brace", false);
    }
}
