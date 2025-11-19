using StateMachineAI;
using UnityEngine;

public class AlertLevel : MonoBehaviour
{

    [Header("Œx‰ú“x‚Ìã¸‘¬“x(1•b‚ ‚½‚è)")]
    public float m_increaseRate = 30f;

    [Header("Œx‰ú“x‚ÌŒ¸­‘¬“x(1•b‚ ‚½‚è)")]
    public float m_decreaseRate = 5f;

    [Header("Œx‰ú“xMAX")]
    public float m_maxLevel = 50f;

    [Header("Œ»İ‚ÌŒx‰ú“x")]
    public float m_currentLevel = 0f;

    [Header("Œx‰úMAX‚ÉAI‚Ö’Ê’m‚·‚é‚©")]
    public bool m_autoAlert = true;

    private EnemyAI enemyAI;
    //UŒ‚ƒ‚[ƒh‚É“ü‚Á‚½‚©‚Ç‚¤‚©
    public bool m_AttackMode=false;



    void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
    }


    public void IncreaseVigilance()
    {
        Debug.Log(111);
        //Œ»İ‚ÌŒx‰ú“x‚ğ‘‰Á
        m_currentLevel += m_increaseRate * Time.deltaTime;
        //Œx‰ú“x‚ÌãŒÀ‚Æ‰ºŒÀ‚ğİ’è
        m_currentLevel = Mathf.Clamp(m_currentLevel, 0, m_maxLevel);
        Debug.Log($"Œx‰ú“xã¸’†: {m_currentLevel:F1}/{m_maxLevel}");
        //ƒŒƒxƒ‹ƒ}ƒbƒNƒX‚æ‚èã‰ñ‚Á‚½‚çUŒ‚ó‘Ô‚Ö
        if (m_autoAlert && m_currentLevel >= m_maxLevel)
        {
            Debug.Log("Œx‰ú“xMAX ¨ UŒ‚ƒ‚[ƒh‚Ö");
            m_AttackMode = true;
            if (enemyAI != null)
                enemyAI.ChangeState(AIState.Attack);
        }
    }


    public void DecreaseVigilance()
    {
        //Œ»İ‚ÌŒx‰ú“x‚ğŒ¸­
        m_currentLevel -= m_decreaseRate * Time.deltaTime;
        //Œx‰ú“x‚ÌãŒÀ‚Æ‰ºŒÀ‚ğİ’è
        m_currentLevel = Mathf.Clamp(m_currentLevel, 0, m_maxLevel);
        Debug.Log($"Œx‰ú“xã¸’†: {m_currentLevel:F1}/{m_maxLevel}");
    }

    //Œx‰ú“x‚ªMax‚É’B‚µ‚½‚çture‚ğ•Ô‚·
    public bool IsMax() => m_currentLevel >= m_maxLevel;
}


