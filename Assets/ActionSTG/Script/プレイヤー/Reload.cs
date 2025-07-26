using UnityEngine;

public class Reload : MonoBehaviour
{
    //リロードアニメーター
    private Animator m_ReloadAnimator;
    //リロード効果音
    public AudioClip m_ReloadSound;
    private AudioSource m_Sound;
    //リロード中は動きを停止（public staticは他のscriptに連動）
    public static bool m_Reloading=false;
    public static int m_GunBullet;
    //scriptのPlayerAttackを参照
    private PlayerAttack m_PlayerAttack;
    private void Start()
    {
        m_ReloadAnimator = GetComponent<Animator>();
        m_Sound = GetComponent<AudioSource>();
        //プレイヤーオブジェクトから PlayerAttack を探す
        m_PlayerAttack = FindObjectOfType<PlayerAttack>();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            m_ReloadAnimator.SetBool("Reload", true);
            m_Sound.PlayOneShot(m_ReloadSound);
            m_Reloading = true;
            GunReload();
        }

    }
    //アニメーションが終了したら動く
    public void ReloadEnd()
    {
        m_ReloadAnimator.SetBool("Reload", false);
        m_Reloading=false;
    }
    void GunReload()
    {
        //弾の補充の数
        m_GunBullet = m_PlayerAttack.m_InitialBulletCount;
        if (m_PlayerAttack != null)
        {
            m_PlayerAttack.m_BulletCount = m_GunBullet;
        }
    }
}
