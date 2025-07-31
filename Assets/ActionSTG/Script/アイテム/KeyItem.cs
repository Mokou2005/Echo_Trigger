using UnityEngine;

public class KeyItem : MonoBehaviour
{
    [Header("鍵の名前")]
    public string KeyName;
    public Animator m_PlayerAnimator;
    //画像
    public GameObject m_Image;
    [Header("鍵を拾う音")]
    public AudioClip m_Keysound;
    private AudioSource m_audioSource;
    //アイテム管理参照
    private ItemManager m_ItemManager;
    //プレイヤーが鍵の近くにいるか
    private bool isPlayerInRange = false;

    private void Start()
    {
        //画像非表示
        m_Image.SetActive(false);
        m_audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            m_PlayerAnimator.SetBool("Push", true);
            //アイテム管理の鍵を追加
            m_ItemManager.AddItem(KeyName);
            //Keyを拾う音
            m_audioSource.PlayOneShot(m_Keysound);
            Debug.Log(KeyName + " を獲得しました");
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            m_PlayerAnimator.SetBool("Push", false);
            Destroy(gameObject,0.7f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //タグがプレイヤーなら
        if (other.CompareTag("Player"))
        {
            m_ItemManager = other.GetComponent<ItemManager>();
            isPlayerInRange = true;
            m_Image.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //タグがプレイヤーなら
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            m_ItemManager = null;
            m_Image.SetActive(false);
        }
    }
}
