using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.AudioSettings;

public class Shooting : MonoBehaviour
{
    [Header("Playerの位置")]
    [SerializeField] private Transform m_Target;
    [Header("外す位置（弾）")]
    //[SerializeField] private Transform[] m_RemoveBullet;
    [Header("Playに当たる確率")]
    [SerializeField] private float m_BulletProbability = 0.8f;
    [Header("射程距離を保つ距離")]
    [SerializeField] private float m_CombatDistance = 5f;
    [Header("射程距離内での移動速度")]
    [SerializeField] private float m_MoveSpeed = 2f;
    [Header("攻撃間隔（秒）")]
    [SerializeField] private float m_ShootInterval = 0.5f;
    [SerializeField] private NavMeshAgent m_agent;
    [SerializeField] private Animator m_Animator;
    //射程距離に入ったかどうか
    private bool m_ShootAria = false;
    //撃った回数
    private int m_Count=0;
    //攻撃の間を図るタイマー
    private float m_ShootTimer=0f;
    //リロードしてるかどうか
    private bool m_IsReloading=false;


    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_agent = GetComponent<NavMeshAgent>();
        // Tagからプレイヤーを自動で探す
        if (m_Target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                m_Target = player.transform;
            }
            else
            {
                Debug.LogWarning("Playerタグのオブジェクトが見つかりません！");
            }
        }
    }


    void Update()
    {


        if (m_Target == null) return;

        float distance = Vector3.Distance(transform.position, m_Target.position);

        if (!m_ShootAria)
        {
            // 射程外なので接近
            m_Animator.SetBool("Run", true);
            m_agent.enabled = true;
            m_agent.speed = 3.5f;
            m_agent.SetDestination(m_Target.position);
        }
        else
        {
            // 射程内：距離を保つ
            m_agent.enabled = true;
            m_agent.speed = m_MoveSpeed;

            if (distance < m_CombatDistance - 1f)
            {
                // 距離を保って攻撃
                m_agent.SetDestination(transform.position);
                m_Animator.SetBool("Run", false);

                // リロード中は攻撃しない
                if (m_IsReloading) return;

                // 一定間隔ごとに攻撃
                m_ShootTimer += Time.deltaTime;
                if (m_ShootTimer >= m_ShootInterval)
                {
                    m_ShootTimer = 0f;
                    

                    if (m_Count< 5)
                    {
                        m_agent.enabled = false;
                        // 攻撃アニメ再生
                        m_Animator.SetTrigger("Attack");
                        m_Count++;
                        Debug.Log($"撃った回数: {m_Count:F1}");
                       
                    }
                    else
                    {
                        m_agent.enabled = false;
                        // リロード開始
                        m_Animator.SetTrigger("Reload");
                        m_IsReloading = true;
                        m_Count = 0;
                        // リロード時間後に解除
                        Invoke(nameof(EndReload), 2f); // ← 2秒後にリロード完了
                    }
                }
            }
            else if (distance > m_CombatDistance + 1f)
            {
                // 射程外 → 近づく
                m_Animator.SetBool("Run", true);
                m_agent.SetDestination(m_Target.position);
            }
        }
    }

    // リロード完了時に呼ばれる
    private void EndReload()
    {
        m_IsReloading = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //エリアに入った
            m_ShootAria = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 射程外に出た
            m_ShootAria = false;
        }
    }
}
