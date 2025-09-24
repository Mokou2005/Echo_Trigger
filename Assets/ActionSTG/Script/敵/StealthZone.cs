using UnityEngine;

using UnityEngine;

public class StealthZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerDetectionState state = other.GetComponent<PlayerDetectionState>();
            if (state != null)
            {
                state.SetInvisible(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerDetectionState state = other.GetComponent<PlayerDetectionState>();
            if (state != null)
            {
                state.SetInvisible(false);
            }
        }
    }
}
