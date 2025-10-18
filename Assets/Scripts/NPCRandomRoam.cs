using UnityEngine;
using UnityEngine.AI;

public class NPCRandomRoam : MonoBehaviour
{
    public float roamRadius = 5f;       // how far they can wander
    public float waitTime = 3f;         // how long to wait between moves
    public bool canMove = true;         // used to freeze NPC during dialogue

    private NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = waitTime;
    }

    void Update()
    {
        if (!canMove) return;

        timer += Time.deltaTime;

        if (timer >= waitTime)
        {
            Vector3 newPos = RandomNavSphere(transform.position, roamRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }

    public void StopMovement()
    {
        if (agent != null)
        {
            agent.isStopped = true;
            canMove = false;
        }
    }

    public void ResumeMovement()
    {
        if (agent != null)
        {
            agent.isStopped = false;
            canMove = true;
        }
    }
}
