using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol_Waypoint : MonoBehaviour
{
    [Header("WaypointManagerのscriptを自動アタッチ")]
    public WaypointManager m_Manager;
    [Header("Sensorを自動アタッチ")]
    public Sensor m_Sensor;
    [Header("AlertLevelを自動アタッチ")]
    public AlertLevel m_AlertLevel;
    [Header("Responを自動アタッチ")]
    [SerializeField] private Respon m_Respon;
    [Header("stoneを自動アタッチ")]
    [SerializeField] private Stone m_stone;
    private NavMeshAgent m_agent;
    //どのWaypointに向かっているか
    private int m_currentIndex = 0;

    [System.Obsolete]
    private void Start()
    {
        m_Sensor = GetComponent<Sensor>();
        m_agent = GetComponent<NavMeshAgent>();
        m_AlertLevel = GetComponent<AlertLevel>();
        //NavMeshの移動速度を設定
        switch (m_Respon.m_UnitType)
        {
            case 0:              
                m_agent.speed = 0f;
                Debug.Log("スーツの速度を設定:" + m_agent.speed);
                break;
            case 1:
                m_agent.speed = 1.5f;
                Debug.Log("警備員の速度を設定:" + m_agent.speed);
                break;
           case 2:
               m_agent.speed = 3f;
                Debug.Log("Dogの速度を設定:" + m_agent.speed);
                break;
            default:
                Debug.LogError("未知のUnityTyoeが来ました。caseを確認してください。");
                break;
        }


        //マネージャーが未設定なら、近くのものを自動検索
        if (m_Manager == null)
        {
            m_Manager = FindClosestManager();
        }
        if (m_Sensor == null)
        {
            Debug.LogError("センサーが入ってません");
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
        if (m_Sensor.m_Look == true && m_AlertLevel.m_AttackMode == false)
        {
            //Navmesh無効
            m_agent.enabled = false;

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

    public void OnDetectStone(Vector3 stonePos)
    {
        //攻撃中など
        if (m_agent.enabled == false) return; 

        m_agent.SetDestination(stonePos);
        //パトロール再開時に最初から
        m_currentIndex = 0; 
    }
    public void SetRespon(Respon respon)
    {
        m_Respon = respon;
        if (m_Respon == null)
        {
            Debug.LogError("Responが入ってません");
        }
    }
    //石の方に向かう関数
    public void StonePatrol()
    {
        GameObject target = GameObject.FindGameObjectWithTag("StoneGetTag");
        if (target!=null)
        {
            // 既に MasterEnemySystem が付いているか確認
            m_stone = target.GetComponent<Stone>();
        }
        if (m_stone!=null)
        {
            //石の方に向かう
            m_agent.SetDestination(m_stone.transform.position);
            
        }
        else
        {
            Debug.Log("石がありません。");
        }
        
    }
    //石の方を向いて停止
    public void StonePatrolStop()
    {

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
