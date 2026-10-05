using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;
    public bool seenPlayer;

    public static event Action OnPlayerSeen;

    private void Start()
    {
        seenPlayer = false;
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    private void OnEnable()
    {
        Enforcer.OnConsequenceComplete += ResetEnforcer;
    }

    private void OnDisable()
    {
        Enforcer.OnConsequenceComplete -= ResetEnforcer;
    }

    private IEnumerator FOVRoutine()
    {
        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;

                    if (!seenPlayer)
                    {
                        OnPlayerSeen?.Invoke();
                        seenPlayer = true;
                    }
 

                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer) 
        {
            canSeePlayer = false;
        }
    }

    // create methid to reset seenPlayer that is triggered via event
    public void ResetEnforcer(Transform player = null)
    {
        canSeePlayer = false;
    }

}
