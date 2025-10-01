using UnityEngine;
using StateMachineAI;

public class Respon : MonoBehaviour
{
    public int m_UnitType = 0;
    public GameObject m_Body;
    public Transform m_Player;
    public void Start()
    {
        SetUp();
    }
    public void SetUp()
    {
        GameObject D = Instantiate(m_Body, transform.position, transform.rotation);
        EnemySuitAI EA = D.GetComponent<EnemySuitAI>();
        EA.RSP = this;
        EA.m_Player=m_Player;

        if(m_UnitType == 1)
        {
            EA.AddStateByName("IdleType");
            EA.AddStateByName("SearchType");
            EA.AddStateByName("AttackType");
        }
        else
        {
            EA.AddStateByName("IdleType");
            EA.AddStateByName("SearchType");
            EA.AddStateByName("AttackType");
        }
        EA.AISetUp();
    }
}
