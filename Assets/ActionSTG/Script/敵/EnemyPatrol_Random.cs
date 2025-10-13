using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol_Random : MonoBehaviour
{
    [Header("敵が動ける半径の範囲")]
    public float m_WalkRadius = 5f;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //最初の位置をランダムに設定
        MoveToRandomPoint();
    }

    public void Move()
    {
        //ナビゲーションが計算中じゃなく五メートルの範囲なら
        if (!agent.pathPending && agent.remainingDistance < 0.5f)

            MoveToRandomPoint();
    }

    void MoveToRandomPoint()
    {
        Vector3 randomDir = Random.insideUnitSphere * m_WalkRadius + transform.position;
        if (NavMesh.SamplePosition(randomDir, out NavMeshHit hit, m_WalkRadius, NavMesh.AllAreas))
            agent.SetDestination(hit.position);
    }
}

