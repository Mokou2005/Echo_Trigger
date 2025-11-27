using System.Collections.Generic;
using UnityEngine;

public class MasterEnemySystem : MonoBehaviour
{
    public List<Respon> m_Respawn;

    public List<GameObject> m_Enemys;
    private void Start()
    {
        foreach (Transform Dummy in transform)
        {
            if (Dummy.GetComponent<Respon>())
            {
                Dummy.GetComponent<Respon>().m_MES = this;
                m_Respawn.Add(Dummy.GetComponent<Respon>());
            }
        }
        m_Enemys.Clear();
    }
    public void EnemyyAdd(GameObject Dummy)
    {
        m_Enemys.Add(Dummy);
    }
}
