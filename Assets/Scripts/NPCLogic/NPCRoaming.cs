using UnityEngine;
using UnityEngine.AI;

public class NPCRoaming : MonoBehaviour
{

    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private float roamRadius = 10f;
    [SerializeField] private float minWaitTime = 2f;
    [SerializeField] private float maxWaitTime = 5f;

    private float waitTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the NavMeshAgent if one wasn't assigned.
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        // Wait a little before choosing the first destination.
        ChooseRandomDestination();
    }

    // Update is called once per frame
    void Update()
    {
        // Don't choose another destination until the NPC
        // has finished walking to its current destination.
        if (DialogueManager_New.Instance.isDialogueActive)
        {
            agent.isStopped = true;
            return;
        }

        agent.isStopped = false;
        if (!agent.pathPending && agent.remainingDistance
            <= agent.stoppingDistance)
        {
            waitTimer += Time.deltaTime;

            // Wait a random amount of time before moving again.
            if (waitTimer >= Random.Range(minWaitTime, maxWaitTime))
            {
                ChooseRandomDestination();
            }
        }
    }

    private void ChooseRandomDestination()
    {
        waitTimer = 0f;

        // Pick a random point near the NPC.
        Vector3 randomDirection =
            Random.insideUnitSphere * roamRadius;

        randomDirection += transform.position;

        // Find the closest valid point on the NavMesh.
        if (NavMesh.SamplePosition(
            randomDirection,
            out NavMeshHit hit,
            roamRadius,
            NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }



}
