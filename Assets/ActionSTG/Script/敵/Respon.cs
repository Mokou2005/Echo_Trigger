using System.Collections.Generic;
using UnityEngine;
using StateMachineAI;
using UnityEngine.Rendering.Universal;


public class Respon : MonoBehaviour
{
    [Header("敵のタイプの変更")]
    public int m_UnitType = 0;
    public GameObject m_Body;
    public Transform m_Player;

    [System.Serializable]

    //リストを二重にする
    public struct AINames
    {
        [Header("敵の名前")]
        public string m_Name;
        public List<string> AIName;
    }

    public List<AINames> m_Ainame;


    public void Start()
    {
        SetUp();
    }
    public void SetUp()
    {
        GameObject D = Instantiate(m_Body, transform.position, transform.rotation);
        EnemyAI EA = D.GetComponent<EnemyAI>();
        EA.RSP = this;
        EA.m_Player = m_Player;
        //指定したタイプのリストがない場合通知する
        if (m_Ainame[m_UnitType].AIName.Count == 0)
        {
            Debug.Log("リストはありますが中身が空です");
        }
        else
        {
            //m_UnitTypeに指定したタイプのステートをdummyにして入れる
            foreach (string dummy in m_Ainame[m_UnitType].AIName)
            {
                EA.AddStateByName(dummy);
            }
        }



        EA.AISetUp();
    }
}
