using UnityEngine;
using UnityEngine.AI;

namespace StateMachineAI
{

    public class AttackSuitType : State<EnemyAI>
    {
        
        public Transform m_Player;
        //味方を読んだかどうか？
        private bool m_hasCalled = false;
        private Animator m_Animator;
        //コンストラクタ
        public AttackSuitType(EnemyAI owner) : base(owner) { }

        public override void Enter()
        {
            m_Animator=owner.GetComponent<Animator>();
        }
        public override void Stay()
        {
            
            // まだ呼んでない場合のみ呼ぶ
            if (!m_hasCalled)
            {
                // 一度だけ呼ぶ
                m_hasCalled = true;

                // 敵を生成
                if (owner.enemyPrefab != null && owner.spawnPoint != null)
                {
                    Debug.Log("仲間を読んでいる！！");
                    m_Animator.SetBool("Hit", true);
                    GameObject enemy = UnityEngine.Object.Instantiate(owner.enemyPrefab, owner.spawnPoint.position, owner.spawnPoint.rotation);

                    // EnemySensor の m_Player にプレイヤーを代入
                    EnemySensor sensor = enemy.GetComponent<EnemySensor>();
                    if (sensor != null && m_Player != null)
                    {
                        sensor.m_Player = m_Player;
                    }
                }
                else
                {
                    Debug.Log("enemyPrefab または spawnPoint が設定されていません");
                }

                // 仲間を移動
                NavMeshAgent agent = owner. m_EnemyCall.GetComponent<NavMeshAgent>();
                if (agent != null && owner.m_suitEnemy != null)
                {
                    agent.SetDestination(owner.m_suitEnemy.position);
                }
            }
        }
        public override void Exit() { }

    }

}
