using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol_Hybrid : MonoBehaviour
{
    public Transform[] m_MainPoints;
    public float m_RandomRadius = 3f;

    private NavMeshAgent agent;
    private int currentPoint = 0;
    private bool isRandomPhase = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        MoveToNextMainPoint();
    }

    public void Move()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (isRandomPhase)
                MoveToNextMainPoint();
            else
                MoveToRandomNearCurrent();
        }
    }

    void MoveToNextMainPoint()
    {
        if (m_MainPoints.Length == 0) return;
        agent.destination = m_MainPoints[currentPoint].position;
        currentPoint = (currentPoint + 1) % m_MainPoints.Length;
        isRandomPhase = false;
    }

    void MoveToRandomNearCurrent()
    {
        Vector3 randomDir = Random.insideUnitSphere * m_RandomRadius + transform.position;
        if (NavMesh.SamplePosition(randomDir, out NavMeshHit hit, m_RandomRadius, NavMesh.AllAreas))
            agent.destination = hit.position;

        isRandomPhase = true;
    }
}
