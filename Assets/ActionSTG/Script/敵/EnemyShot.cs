using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    [Header("プレイヤーが持つ銃")]
    public Transform m_Gun;
    [Header("弾複製（プレハブ）")]
    public GameObject m_bulletPrefab;
    [Header("弾の速さ")]
    public float m_BulletSpeed;
    public AudioClip m_sound;


    public void Shot()
    {
            //銃口の設定（銃口から発射）
            GameObject Bullet = Instantiate(m_bulletPrefab, m_Gun.position, m_Gun.rotation);
            //上のBulletにRigidbodyを格納（力を加えるなど）
            Rigidbody BulletRb = Bullet.GetComponent<Rigidbody>();
            //弾が撃たれる方向とスピード
            BulletRb.AddForce(m_Gun.forward * m_BulletSpeed);
            //発射音
            AudioSource.PlayClipAtPoint(m_sound, transform.position);
            //五秒後削除
            Destroy(Bullet, 5.0f);


    }
}
