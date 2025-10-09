using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol_Waypoint : MonoBehaviour
{
    [Header("WaypointManagerのscriptをアタッチ")]
    public WaypointManager m_Manager;
    private NavMeshAgent m_agent;
    //
    private int m_currentIndex = 0;

    private void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        //マネージャーが未設定なら、近くのものを自動検索
        if (m_Manager == null)
        {
            //
            m_Manager = FindClosestManager(); ;
        }
        //
        MoveToNextPoint();
    }

    private void Update()
    {
        //
        if (!m_agent.pathPending && m_agent.remainingDistance < 0.5f)
            MoveToNextPoint();
    }
    void MoveToNextPoint()
    {
        if (m_Manager == null || m_Manager.m_Waypoints.Length == 0)
        {
            Debug.Log("WaypointManagerのscriptが原因です。");
            return;
        }
        //
        m_agent.destination = m_Manager.m_Waypoints[m_currentIndex].position;
        m_currentIndex = (m_currentIndex + 1) % m_Manager.m_Waypoints.Length;
    }
    WaypointManager FindClosestManager()
    {
        WaypointManager[] managers = FindObjectsOfType<WaypointManager>();
        WaypointManager closest = null;
        float minDist = Mathf.Infinity;

        foreach (var m in managers)
        {
            float dist = Vector3.Distance(transform.position, m.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = m;
            }
        }
        return closest;
    }
}
