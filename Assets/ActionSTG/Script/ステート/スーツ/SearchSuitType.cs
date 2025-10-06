using Unity.VisualScripting;
using UnityEngine;

namespace StateMachineAI
{

    public class SearchSuitType : State<EnemyAI>
    {
        [Header("警戒度の上昇速度(1秒あたり)")]
        public float m_VigilanceLevelIncreaseCount = 30f;
        [Header("警戒度の減少速度（1秒あたり）")]
        public float m_VigilanceLevelDecreaseCount = 5f;
        //警戒度
        private float m_VigilanceLevel = 0f;
        //警戒度MAX
        private float m_VigilanceLevelMax = 100f;
        private Animator m_Animator;
        //script参照
        private PlayerDetectionState m_playerDetectionState;
        //コンストラクタ
        public SearchSuitType(EnemyAI owner) : base(owner) { }

        public override void Enter()
        {
            m_Animator = owner.GetComponent<Animator>();
        }
        public override void Stay()
        {
            //見えない状態
            bool isInvisible = m_playerDetectionState != null && m_playerDetectionState.IsInvisible();
            // 敵からプレイヤーの方向と距離
            Vector3 toPlayer = owner.m_Player.position - owner.transform.position;
            float distance = toPlayer.magnitude;
            bool isInView = false;
            if (!isInvisible && distance <= owner.m_viewDistance)
            {
                //敵の視野角の判定
                Vector3 forward = owner.transform.forward;
                Vector3 dirToPlayer = toPlayer.normalized;
                float dot = Vector3.Dot(forward, dirToPlayer);
                float threshold = Mathf.Cos(owner.m_viewAngle * 0.5f * Mathf.Deg2Rad);
                //視野に入ったら
                if (dot >= threshold)
                {
                    Debug.Log("疑っている");
                    m_Animator.SetBool("Search", true);
                    //警戒度を加算
                    m_VigilanceLevel += m_VigilanceLevelIncreaseCount * Time.deltaTime;
                    //警戒度の上限を設定
                    m_VigilanceLevel = Mathf.Clamp(m_VigilanceLevel, 0f, m_VigilanceLevelMax);
                    Debug.Log($"警戒中.警戒度:{m_VigilanceLevel}");
                    //警戒度が100に達したら
                    if (m_VigilanceLevel >= m_VigilanceLevelMax)
                    {
                        Debug.Log("見つかってしまった！！");
                        owner.ChangeState(AIState.AttackSuitType);
                    }
                }
                else
                {

                    Debug.Log("プレイヤーは見えない");

                    //警戒度減少
                    m_VigilanceLevel -= m_VigilanceLevelDecreaseCount * Time.deltaTime;
                    //警戒度の上限設定
                    m_VigilanceLevel = Mathf.Clamp(m_VigilanceLevel, 0f, m_VigilanceLevelMax);
                    Debug.Log($"警戒中.警戒度:{m_VigilanceLevel}");
                    if(m_VigilanceLevel<=5){
                        Debug.Log("警戒モードリセット初期状態");
                        m_Animator.SetBool("Search", false);
                        owner.ChangeState(AIState.IdleSuitType);
                    }
                }

            }
        }
        public override void Exit() { }
    }

}
