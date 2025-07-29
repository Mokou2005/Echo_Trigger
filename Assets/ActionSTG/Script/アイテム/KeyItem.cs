using UnityEngine;

public class KeyItem : MonoBehaviour
{
    [Header("Œ®‚Ì–¼‘O")]
    public string KeyName;
    //‰æ‘œ
    public GameObject m_Image;
    private ItemManager m_ItemManager;
    private bool isPlayerInRange = false;

    private void Start()
    {
        m_Image.SetActive(false);
    }
    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            m_ItemManager.AddItem(KeyName);
            Debug.Log(KeyName + " ‚ðŠl“¾‚µ‚Ü‚µ‚½");
            Destroy(gameObject);
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
