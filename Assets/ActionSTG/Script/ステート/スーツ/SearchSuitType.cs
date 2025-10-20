/*using UnityEngine;

namespace StateMachineAI
{
    public class SearchSuitType : State<EnemyAI>
    {
        [Header("警戒度の上昇速度(1秒あたり)")]
        public float m_VigilanceLevelIncreaseCount = 30f;

        [Header("警戒度の減少速度（1秒あたり）")]
        public float m_VigilanceLevelDecreaseCount = 5f;

        private float m_VigilanceLevel = 0f;
        private float m_VigilanceLevelMax = 100f;

        private Animator m_Animator;
        private Sensor m_Sensor; // ← Sensorスクリプト参照

        // コンストラクタ
        public SearchSuitType(EnemyAI owner) : base(owner) { }

        public override void Enter()
        {
            m_Animator = owner.GetComponent<Animator>();
            m_Sensor = owner.GetComponent<Sensor>();

            if (m_Sensor == null)
            {
                Debug.LogWarning($"Sensorが{owner.name}にアタッチされていません。視覚検知ができません。");
            }

            Debug.Log($"[{owner.name}] SearchSuitType開始");
        }

        public override void Stay()
        {
            // Sensorがプレイヤーを検出しているか
            bool playerDetected = m_Sensor != null && m_Sensor.IsPlayerVisible();

            if (playerDetected)
            {
                m_Animator.SetBool("Search", true);
                m_VigilanceLevel += m_VigilanceLevelIncreaseCount * Time.deltaTime;
                m_VigilanceLevel = Mathf.Clamp(m_VigilanceLevel, 0f, m_VigilanceLevelMax);
                Debug.Log($"警戒中（視認中）: {m_VigilanceLevel}");

                if (m_VigilanceLevel >= m_VigilanceLevelMax)
                {
                    Debug.Log("プレイヤーを完全に発見！");
                    owner.ChangeState(AIState.Attack);
                }
            }
            else
            {
                m_VigilanceLevel -= m_VigilanceLevelDecreaseCount * Time.deltaTime;
                m_VigilanceLevel = Mathf.Clamp(m_VigilanceLevel, 0f, m_VigilanceLevelMax);
                Debug.Log($"警戒中（未視認）: {m_VigilanceLevel}");

                if (m_VigilanceLevel <= 5f)
                {
                    Debug.Log("警戒解除 → Idleへ戻る");
                    m_Animator.SetBool("Search", false);
                    owner.ChangeState(AIState.Idle);
                }
            }
        }

        public override void Exit()
        {
            m_Animator.SetBool("Search", false);
            Debug.Log($"[{owner.name}] SearchSuitType終了");
        }
    }
}*/

