using UnityEngine;
using UnityEngine.AI;

public class NPCRoaming : MonoBehaviour
{

    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private float roamRadius = 10f;
    [SerializeField] private float minWaitTime = 2f;
    [SerializeField] private float maxWaitTime = 5f;

    private float waitTimer;

    private Vector3 startingPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the NavMeshAgent if one wasn't assigned.
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        startingPosition = transform.position;
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

        // Choose a random point within the radius
        // of the NPC's original starting position.
        Vector2 randomCircle = Random.insideUnitCircle * roamRadius;

        Vector3 randomPosition = new Vector3(
            startingPosition.x + randomCircle.x,
            startingPosition.y,
            startingPosition.z + randomCircle.y
        );

        // Find a valid point on the NavMesh near that position.
        if (NavMesh.SamplePosition(
            randomPosition,
            out NavMeshHit hit,
            2f,
            NavMesh.AllAreas))
        {
            // Make sure the destination is still within
            // the allowed radius of the starting position.
            Vector3 destinationOffset = hit.position - startingPosition;
            destinationOffset.y = 0f;

            if (destinationOffset.magnitude <= roamRadius)
            {
                agent.SetDestination(hit.position);
            }
        }



        //waitTimer = 0f;

        //// Pick a random point near the NPC.
        //Vector3 randomDirection =
        //    Random.insideUnitSphere * roamRadius;

        //randomDirection += transform.position;

        //// Find the closest valid point on the NavMesh.
        //if (NavMesh.SamplePosition(
        //    randomDirection,
        //    out NavMeshHit hit,
        //    roamRadius,
        //    NavMesh.AllAreas))
        //{
        //    agent.SetDestination(hit.position);
        //}
    }



}
