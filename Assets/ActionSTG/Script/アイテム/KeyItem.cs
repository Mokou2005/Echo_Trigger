using UnityEngine;

public class KeyItem : MonoBehaviour
{
    [Header("鍵の名前")]
    public string KeyName;
    public Animator m_PlayerAnimator;
    public GameObject m_Image;
    [Header("鍵を拾う音")]
    public AudioClip m_Keysound;
    private AudioSource m_audioSource;

    private ItemManager m_ItemManager;
    private bool isPlayerInRange = false;

    private void Start()
    {
        m_Image.SetActive(false);
        m_audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // プレイヤーが近くにいて、Eキーが押されたら
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            m_PlayerAnimator.SetBool("Push", true);
            if (m_ItemManager != null)
            {
                m_ItemManager.AddItem(KeyName);
                m_audioSource.PlayOneShot(m_Keysound);
                Debug.Log(KeyName + " を獲得しました");
            }
        }

        // Eキーを離したら
        if (isPlayerInRange && Input.GetKeyUp(KeyCode.E))
        {
            m_PlayerAnimator.SetBool("Push", false);
            Destroy(gameObject, 0.7f); // 0.7秒後に鍵オブジェクトを消す
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_ItemManager = other.GetComponent<ItemManager>();
            isPlayerInRange = true;
            m_Image.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            m_ItemManager = null;
            m_Image.SetActive(false);
        }
    }
}
