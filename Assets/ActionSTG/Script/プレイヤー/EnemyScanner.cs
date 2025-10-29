using UnityEngine;
using System.Collections;

public class EnemyScanner : MonoBehaviour
{
    public Material scanMat;
    public float speed = 10f;
    private bool isScanning = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isScanning)
        {
            StartCoroutine(Scan());
        }
    }

    IEnumerator Scan()
    {
        isScanning = true;
        scanMat.SetFloat("_Scan", 1);
        scanMat.SetFloat("_ScanRadius", 0);  // Å© ÉäÉZÉbÉgí«â¡

        float radius = 0f;
        while (radius < 100f)
        {
            radius += Time.deltaTime * speed;
            scanMat.SetFloat("_ScanRadius", radius);
            yield return null;
        }

        scanMat.SetFloat("_Scan", 0);
        isScanning = false;
    }
}
