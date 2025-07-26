using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    public float patrolRadius = 5f;
    public float patrolInterval = 3f;
    public Transform patrolCenter; // © ’Ç‰ÁIœpœj‚Ì’†S‚Æ‚È‚éˆÊ’u

    private NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = patrolInterval;
    }

    void Update()
    {
        Vector3 direction = agent.velocity.normalized;
        if (direction.sqrMagnitude > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            lookRotation.x = 0;
            lookRotation.z = 0;
            transform.rotation = lookRotation;
        }
        timer += Time.deltaTime;
        if (timer >= patrolInterval && agent.remainingDistance < 0.5f)
        {
            Vector3 newPos = RandomNavmeshLocation(patrolRadius);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += patrolCenter.position; // © œpœj‚Ì’†S‚ğw’èI
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas);
        return hit.position;
    }
}
