using UnityEngine;
using UnityEngine.AI;


public class SoldatoScript : MonoBehaviour
{

    // core stages (4):
    // 1. Chase
    // 2. Stomp
    // 3. Charge wind up
    // 4. Charge attack

    public Transform player;
    public NavMeshAgent agent;

    public float stompRange = 2f;
    public float stompCooldown = 1.5f;

    public float chargeCooldown = 5f;
    public float chargeSpeed = 15f;
    public float chargeDuration = 1.5f;

    private float stompTimer;
    private float chargeTimer;

    float windupTime = 3f;
    float windupTimer = 0f;

    float dashTimer = 0f;

    private enum State { Chase, Stomp, ChargeWindup, ChargeDash }
    private State currentState;

    private Vector3 chargeDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = State.Chase;

    }

    // Update is called once per frame
    void Update()
    {
        stompTimer += Time.deltaTime;
        chargeTimer += Time.deltaTime;

        // movement test
        // agent.SetDestination(player.position);


        switch (currentState)
        {
            case State.Chase:
                Chase();
                break;

            case State.Stomp:
                Stomp();
                break;

            case State.ChargeWindup:
                ChargeWindup();
                break;

            case State.ChargeDash:
                ChargeDash();
                break;
        }



    }

    void Chase()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= stompRange)
        {
            currentState = State.Stomp;
            Debug.Log("Stomping!"); // move to stomp method?
            return;
        }

        if (chargeTimer >= chargeCooldown)
        {
            currentState = State.ChargeWindup;
            agent.isStopped = true;
        }

    }

    void Stomp()
    {
        agent.isStopped = true;

        transform.LookAt(player);

        if (stompTimer >= stompCooldown)
        {
            stompTimer = 0f;
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= stompRange)
            {
                Debug.Log("Player takes stomp damage");
                // Apply damage here
            }
            else
            {
                currentState = State.Chase;
            }


        }
    }

    void ChargeWindup()
    {

        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.ResetPath(); // stops boss

        windupTimer += Time.deltaTime;

        // rotating towards player
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {

            transform.rotation = Quaternion.LookRotation(direction);

        }

        if (windupTimer >= windupTime)
        {

            windupTimer = 0f;
            chargeTimer = 0f;

            chargeDirection = transform.forward;

            currentState = State.ChargeDash;

        }


    }

    void ChargeDash()
    {

        //agent.isStopped = false;

        dashTimer += Time.deltaTime;

        agent.isStopped = true;

        transform.position += chargeDirection * chargeSpeed * Time.deltaTime;

        if (dashTimer >= chargeDuration) 
        {
            dashTimer = 0f;
            currentState = State.Chase;
        }
    }


}
