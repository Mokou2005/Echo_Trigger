using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol_Waypoint : MonoBehaviour
{
    [Header("WaypointManagerのscriptをアタッチ")]
    public WaypointManager m_Manager;
    private NavMeshAgent m_agent;
    //どのWaypointに向かっているか
    private int m_currentIndex = 0;

    private void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        //マネージャーが未設定なら、近くのものを自動検索
        if (m_Manager == null)
        {
            m_Manager = FindClosestManager(); ;
        }
        ////次のPointへ
        MoveToNextPoint();
    }

    private void Update()
    {
        //NavMeshAgentがまだ経路を計算中ではなく現在の目的地に到着したら
        if (!m_agent.pathPending && m_agent.remainingDistance < 0.5f)
            //次のPointへ
            MoveToNextPoint();
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
    WaypointManager FindClosestManager()
    {
        //シーン内に存在する全ての WaypointManager コンポーネントを配列で取得
        WaypointManager[] managers = FindObjectsOfType<WaypointManager>();
        //初期化
        WaypointManager closest = null;
        float minDist = Mathf.Infinity;

        //WaypointManagerを一つずついれる（mに）
        foreach (var m in managers)
        {
            //敵とポイント（移動座標）の間の距離
            float dist = Vector3.Distance(transform.position, m.transform.position);
            //敵からポインターが一番近いものを入れる
            if (dist < minDist)
            {
                //近いものに更新
                minDist = dist;
                //ｍを記録
                closest = m;
            }
        }
        return closest;
    }
}
