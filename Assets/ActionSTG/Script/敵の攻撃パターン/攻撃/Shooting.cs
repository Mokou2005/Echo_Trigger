using UnityEngine;
using UnityEngine.AI;

public class Shooting : MonoBehaviour
{
    [Header("Playerの位置")]
    [SerializeField] private Transform m_Target;
    [Header("外す位置（弾）")]
    [SerializeField] private Transform[] m_RemoveBullet;
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
    [Header("銃（AssaultRifle）")]
    [SerializeField] private AssaultRifle m_Rifle;

    private bool m_IsReloading = false;
    private int m_Count = 0;
    private float m_ShootTimer = 0f;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_agent = GetComponent<NavMeshAgent>();

        if (m_Rifle == null)
            m_Rifle = GetComponentInChildren<AssaultRifle>();

        if (m_Target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null) m_Target = player.transform;
            else Debug.LogWarning("Playerタグのオブジェクトが見つかりません！");
        }
    }

    void Update()
    {
        if (m_Target == null) return;

        float distance = Vector3.Distance(transform.position, m_Target.position);
        bool inCombatRange = distance <= m_CombatDistance + 1f; // 射程圏内か判定

        if (!inCombatRange)
        {
            // 射程外 → 接近
            m_Animator.SetBool("Run", true);
            m_agent.enabled = true;
            m_agent.speed = 3.5f;
            m_agent.SetDestination(m_Target.position);
            return;
        }

        // 射程内：距離を保ちながら攻撃
        m_agent.enabled = true;
        m_agent.speed = m_MoveSpeed;

        Vector3 lookPos = m_Target.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);

        if (distance < m_CombatDistance - 1f)
        {
            m_agent.SetDestination(transform.position);
            m_Animator.SetBool("Run", false);

            if (m_IsReloading) return;

            m_ShootTimer += Time.deltaTime;
            if (m_ShootTimer >= m_ShootInterval)
            {
                m_ShootTimer = 0f;

                if (m_Count < 5)
                {
                    m_agent.enabled = false;
                    m_Animator.SetTrigger("Attack");
                    m_Count++;
                    Shot();
                }
                else
                {
                    m_agent.enabled = false;
                    m_Animator.SetTrigger("Reload");
                    m_IsReloading = true;
                    m_Count = 0;
                    Invoke(nameof(EndReload), 2f);
                }
            }
        }
        else if (distance > m_CombatDistance + 1f)
        {
            // 射程外 → 接近
            m_Animator.SetBool("Run", true);
            m_agent.SetDestination(m_Target.position);
        }
    }

    private void Shot()
    {
        if (m_Rifle != null)
        {
            m_Rifle.Shot();
            Debug.Log("弾を発射しました！");
        }
        else
        {
            Debug.LogWarning("AssaultRifle が設定されていません！");
        }
    }

    private void EndReload()
    {
        m_IsReloading = false;
    }
}
