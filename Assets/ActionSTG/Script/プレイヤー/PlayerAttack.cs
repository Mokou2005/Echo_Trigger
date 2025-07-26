using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.ParticleSystemJobs;


public class PlayerAttack : MonoBehaviour
{
    [Header("撃った時のアニメーション")]
    public Transform m_effect;
    [Header("プレイヤーが持つ銃")]
    public Transform m_Gun;
    [Header("弾複製（プレハブ）")]
    public GameObject m_bulletPrefab;
    [Header("弾の速さ")]
    public float bulletSpeed;
    [Header("撃つ効果音")]
    public AudioClip m_soundClip;
    [Header("弾数")]
    public int m_BulletCount;
    [Header("初期弾数(リロード数)")]
    public int m_InitialBulletCount;
    //撃つアニメーション
    private Animator m_animator;
    //効果音
    private AudioSource m_audioSource;

    void Start()
    {
        //格納
        m_audioSource = gameObject.AddComponent<AudioSource>();
        m_animator = GetComponent<Animator>();
        //開始時は非表示
        if (m_effect != null)
            m_effect.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonUp(0) )
        {
            if (m_BulletCount > 0)
            {
                //撃つアニメーション
                if (m_animator != null)
                    m_animator.SetTrigger("Shoot");
                //弾のエフェクト
                if (m_effect != null)
                    m_effect.gameObject.SetActive(true);
                //撃つ効果音
                m_audioSource.PlayOneShot(m_soundClip);
                //弾の発射
                Shoot();
            }

        }
    }

    void Shoot()
    {
        //m_Countが0なら撃つのをやめる（弾がなくなったら)
        --m_BulletCount;
        if (m_BulletCount > -1)
        {
            //弾のエフェクト
            if (m_effect != null)
            {
                m_effect.GetComponent<ParticleSystem>().Stop();
                m_effect.GetComponent<ParticleSystem>().Play();
            }

            //弾作成
            GameObject bullet = Instantiate(m_bulletPrefab, m_Gun.position, m_Gun.rotation);
            // 前方に飛ばす
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            //弾にDamegeSystemスクリプトがついていたら
            if (bullet.GetComponent<DamegeSystem>())
            {
               //誰が撃ったかパラメータで確認（味方同士がダメージを食らわないため）
                bullet.GetComponent<DamegeSystem>().m_Parameta = GetComponent<Parameta>();

            }
            if (rb != null)
            {
                //弾を正面に真っすぐ飛ばす（AddForceは力を加える）
                rb.AddForce(m_Gun.forward * bulletSpeed);
            }

            //五秒後に消える
            Destroy(bullet, 5.0f);

        }
    }
}

