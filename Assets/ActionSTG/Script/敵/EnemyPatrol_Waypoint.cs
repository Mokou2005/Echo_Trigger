using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol_Waypoint : MonoBehaviour
{
    [Header("WaypointManagerのscriptをアタッチ")]
    public WaypointManager m_Manager;
    [Header("Sensorをアタッチ")]
    public Sensor m_Sensor;
    [Header("AlertLevelをアタッチ")]
    public AlertLevel m_AlertLevel;
    private NavMeshAgent m_agent;
    //どのWaypointに向かっているか
    private int m_currentIndex = 0;

    [System.Obsolete]
    private void Start()
    {
        m_Sensor=GetComponent<Sensor>();
        m_agent = GetComponent<NavMeshAgent>();
        m_AlertLevel = GetComponent<AlertLevel>();
        //マネージャーが未設定なら、近くのものを自動検索
        if (m_Manager == null)
        {
            m_Manager = FindClosestManager(); ;
        }
        if (m_Sensor==null)
        {
            Debug.Log("センサーが入ってません");
        }
        ////次のPointへ
        MoveToNextPoint();
    }

    private void Update()
    {
        if (m_agent.enabled)
        {
            //NavMeshAgentがまだ経路を計算中ではなく現在の目的地に到着したら
            if (!m_agent.pathPending && m_agent.remainingDistance < 0.5f)
                //次のPointへ
                MoveToNextPoint();
        }

        //センサーが反応したら
        if (m_Sensor.m_Look==true&&m_AlertLevel.m_AttackMode==false)
        {
            //Navmesh無効
           m_agent.enabled =false;

        }
        if (m_Sensor.m_Look == false && m_AlertLevel.m_AttackMode == false)
        {
            m_agent.enabled = true;
        }
    }
    void MoveToNextPoint()
    {
        if (m_Manager == null || m_Manager.m_Waypoints.Length == 0)
        {
            Debug.Log("WaypointManagerのscriptが原因です。");
            return;
        }
        //次の移動ポイントをセット
        m_agent.destination = m_Manager.m_Waypoints[m_currentIndex].position;
        //配列の最後まで行ったら最初に戻る
        m_currentIndex = (m_currentIndex + 1) % m_Manager.m_Waypoints.Length;
    }


    // 最も近いManagerを探す
    [System.Obsolete]
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
