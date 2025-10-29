using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.ParticleSystemJobs;
using UnityEngine.UI;


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
    [Header("標準をUIに")]
    public Image m_image;
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
    [System.Obsolete]
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

    [System.Obsolete]
    void Shoot()
    {
        --m_BulletCount;
        if (m_BulletCount < 0) return;

        // エフェクト
        if (m_effect != null)
        {
            m_effect.GetComponent<ParticleSystem>().Stop();
            m_effect.GetComponent<ParticleSystem>().Play();
        }

        // Ray発射（画面中心 or クロスヘアUI位置）
        Vector3 screenPoint = new Vector3(Screen.width / 2f, Screen.height / 2f);
        if (m_image != null) screenPoint = m_image.rectTransform.position;

        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 200f))
        {
            // 敵や壁の表面に向けて撃つ
            targetPoint = hitInfo.point;
        }
        else
        {
            // 遠くの仮ターゲットに向けて撃つ
            targetPoint = ray.origin + ray.direction * 200f;
        }

        // 弾生成
        GameObject bullet = Instantiate(m_bulletPrefab, m_Gun.position, Quaternion.identity);

        // ターゲットへ真っすぐ飛ばす
        Vector3 shootDirection = (targetPoint - m_Gun.position).normalized;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = shootDirection * bulletSpeed; // VelocityChangeより安定
        }

        // ダメージ設定
        DamegeSystem dmg = bullet.GetComponent<DamegeSystem>();
        if (dmg != null)
            dmg.m_Parameta = GetComponent<Parameta>();

        Destroy(bullet, 5f);

    }
    
}

