using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol_Random : MonoBehaviour
{
    public float m_WalkRadius = 5f;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        MoveToRandomPoint();
    }

    public void Move()
    {
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

